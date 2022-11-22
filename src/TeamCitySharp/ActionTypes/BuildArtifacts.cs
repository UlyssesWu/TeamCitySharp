﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using TeamCitySharp.Connection;
using System.Threading.Tasks;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;
using File = System.IO.File;

namespace TeamCitySharp.ActionTypes
{
    internal class BuildArtifacts : IBuildArtifacts
    {
        private readonly ITeamCityCaller m_caller;

        public BuildArtifacts(ITeamCityCaller caller)
        {
            m_caller = caller;
        }

        public void DownloadArtifactsByBuildId(string buildId, Action<string> downloadHandler)
        {
            m_caller.GetDownloadFormat(downloadHandler, "/downloadArtifacts.html?buildId={0}", false, buildId);
        }

        public async Task DownloadArtifactsByBuildIdAsync(string buildId, Action<string> downloadHandler)
        {
            await m_caller.GetDownloadFormatAsync(downloadHandler, "/downloadArtifacts.html?buildId={0}", false, buildId);
        }

        public async Task<ArtifactFiles> GetArtifactsAsync(string buildId, string subPath = "")
        {
            var artifacts = await m_caller.GetAsync<ArtifactFiles>($"/builds/{buildId}/artifacts/children/{subPath}");
            return artifacts;
        }

        public async Task<ArtifactFiles> GetArtifactsByLocatorAsync(BuildLocator locator, string subPath = "")
        {
            var artifacts = await m_caller.GetAsync<ArtifactFiles>($"/builds/{locator}/artifacts/children/{subPath}");
            return artifacts;
        }

        public async Task<ArtifactFiles> GetArtifactsAsync(ArtifactItem item)
        {
            var artifacts = await m_caller.GetAsync<ArtifactFiles>(item.Href, false);
            return artifacts;
        }

        public async Task<byte[]> DownloadArtifactAsync(string href)
        {
            return await m_caller.GetByteArrayAsync(href);
        }

        public async Task<byte[]> DownloadArtifactAsync(ArtifactItem item)
        {
            return await m_caller.GetByteArrayAsync(item.Content.Href);
        }

        public ArtifactWrapper ByBuildConfigId(string buildConfigId, string param = "")
        {
            return new ArtifactWrapper(m_caller, buildConfigId, param);
        }
    }

    public class ArtifactWrapper
    {
        private readonly ITeamCityCaller m_caller;
        private readonly string m_buildConfigId;
        private readonly string m_param;

        internal ArtifactWrapper(ITeamCityCaller caller, string buildConfigId, string param)
        {
            m_caller = caller;
            m_buildConfigId = buildConfigId;
            m_param = param;
        }

        public ArtifactCollection LastFinished()
        {
            return Specification(".lastFinished");
        }

        public async Task<ArtifactCollection> LastFinishedAsync()
        {
            return await SpecificationAsync(".lastFinished");
        }

        public ArtifactCollection LastPinned()
        {
            return Specification(".lastPinned");
        }

        public async Task<ArtifactCollection> LastPinnedAsync()
        {
            return await SpecificationAsync(".lastPinned");
        }

        public ArtifactCollection LastSuccessful()
        {
            return Specification(".lastSuccessful");
        }

        public async Task<ArtifactCollection> LastSuccessfulAsync()
        {
            return await SpecificationAsync(".lastSuccessful");
        }

        public ArtifactCollection Tag(string tag)
        {
            return Specification(tag + ".tcbuildid");
        }

        public async Task<ArtifactCollection> TagAsync(string tag)
        {
            return await SpecificationAsync(tag + ".tcbuildid");
        }

        public ArtifactCollection Specification(string buildSpecification)
        {
            var url = $"/repository/download/{m_buildConfigId}/{buildSpecification}/teamcity-ivy.xml";
            var xml = m_caller.GetRaw(string.IsNullOrEmpty(m_param) ? url : $"{url}?{m_param}", false);

            var document = new XmlDocument();
            document.LoadXml(xml);
            var artifactNodes = document.SelectNodes("//artifact");
            if (artifactNodes == null)
                return null;
            var list = new List<string>();
            foreach (XmlNode node in artifactNodes)
            {
                var nameNode = node.SelectSingleNode("@name");
                var extensionNode = node.SelectSingleNode("@ext");
                var artifact = string.Empty;
                if (nameNode != null)
                    artifact = nameNode.Value;
                if (extensionNode != null)
                    artifact += "." + extensionNode.Value;
                list.Add($"/repository/download/{m_buildConfigId}/{buildSpecification}/{artifact}");
            }

            return new ArtifactCollection(m_caller, list, m_param);
        }

        private async Task<ArtifactCollection> SpecificationAsync(string buildSpecification)
        {
            var url = $"/repository/download/{m_buildConfigId}/{buildSpecification}/teamcity-ivy.xml";
            var xml = await m_caller.GetRawAsync(string.IsNullOrEmpty(m_param) ? url : $"{url}?{m_param}", false);

            var document = new XmlDocument();
            document.LoadXml(xml);
            var artifactNodes = document.SelectNodes("//artifact");
            if (artifactNodes == null)
                return null;
            var list = new List<string>();
            foreach (XmlNode node in artifactNodes)
            {
                var nameNode = node.SelectSingleNode("@name");
                var extensionNode = node.SelectSingleNode("@ext");
                var artifact = string.Empty;
                if (nameNode != null)
                    artifact = nameNode.Value;
                if (extensionNode != null)
                    artifact += "." + extensionNode.Value;
                list.Add($"/repository/download/{m_buildConfigId}/{buildSpecification}/{artifact}");
            }

            return new ArtifactCollection(m_caller, list, m_param);
        }
    }

    public class ArtifactCollection
    {
        private readonly ITeamCityCaller m_caller;
        private readonly List<string> m_urls;
        private readonly string m_param;

        internal ArtifactCollection(ITeamCityCaller caller, List<string> urls, string param = "")
        {
            m_caller = caller;
            m_urls = urls;
            m_param = param;
        }

        public List<string> GetArtifactUrl()
        {
            return m_urls;
        }

        /// <summary>
        /// Takes a list of artifact urls and downloads them, see ArtifactsBy* methods.
        /// </summary>
        /// <param name="directory">
        /// Destination directory for downloaded artifacts, default is current working directory.
        /// </param>
        /// <param name="flatten">
        /// If <see langword="true"/> all files will be downloaded to destination directory, no subfolders will be created.
        /// </param>
        /// <param name="overwrite">
        /// If <see langword="true"/> files that already exist where a downloaded file is to be placed will be deleted prior to download.
        /// </param>
        /// <returns>
        /// A list of full paths to all downloaded artifacts.
        /// </returns>
        public List<string> Download(string directory = null, bool flatten = false, bool overwrite = true)
        {
            if (directory == null)
                directory = Directory.GetCurrentDirectory();
            var downloaded = new List<string>();
            foreach (var url in m_urls)
            {
                // user probably didnt use to artifact url generating functions
                Debug.Assert(url.StartsWith("/repository/download/"));

                // figure out local filename
                var parts = url.Split('/').Skip(5).ToArray();
                var destination = flatten
                    ? parts.Last()
                    : string.Join(Path.DirectorySeparatorChar.ToString(), parts);
                destination = Path.Combine(directory, destination);

                // create directories that doesnt exist
                var directoryName = Path.GetDirectoryName(destination);
                if (directoryName != null && !Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                // add artifact to list regardless if it was downloaded or skipped
                downloaded.Add(Path.GetFullPath(destination));

                // if the file already exists delete it or move to next artifact
                if (File.Exists(destination))
                {
                    if (overwrite) File.Delete(destination);
                    else continue;
                }

                var currentUrl = url;
                if (!string.IsNullOrEmpty(m_param))
                {
                    currentUrl = $"{currentUrl}?{m_param}";
                }

                m_caller.GetDownloadFormat(tempFile => File.Move(tempFile, destination), currentUrl, false);
            }

            return downloaded;
        }

        public async Task<List<string>> DownloadAsync(string directory = null, bool flatten = false, bool overwrite = true)
        {
            if (directory == null)
                directory = Directory.GetCurrentDirectory();
            var downloaded = new List<string>();
            foreach (var url in m_urls)
            {
                // user probably didnt use to artifact url generating functions
                Debug.Assert(url.StartsWith("/repository/download/"));

                // figure out local filename
                var parts = url.Split('/').Skip(5).ToArray();
                var destination = flatten
                    ? parts.Last()
                    : string.Join(Path.DirectorySeparatorChar.ToString(), parts);
                destination = Path.Combine(directory, destination);

                // create directories that doesnt exist
                var directoryName = Path.GetDirectoryName(destination);
                if (directoryName != null && !Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                // add artifact to list regardless if it was downloaded or skipped
                downloaded.Add(Path.GetFullPath(destination));

                // if the file already exists delete it or move to next artifact
                if (File.Exists(destination))
                {
                    if (overwrite) File.Delete(destination);
                    else continue;
                }

                var currentUrl = url;
                if (!string.IsNullOrEmpty(m_param))
                {
                    currentUrl = $"{currentUrl}?{m_param}";
                }

                await m_caller.GetDownloadFormatAsync(tempFile => File.Move(tempFile, destination), currentUrl, false);
            }

            return downloaded;
        }

        /// <summary>
        /// Takes a list of artifact urls and downloads them, see ArtifactsBy* methods.
        /// </summary>
        /// <param name="directory">
        /// Destination directory for downloaded artifacts, default is current working directory.
        /// </param>
        /// <param name="flatten">
        /// If <see langword="true"/> all files will be downloaded to destination directory, no subfolders will be created.
        /// </param>
        /// <param name="overwrite">
        /// If <see langword="true"/> files that already exist where a downloaded file is to be placed will be deleted prior to download.
        /// </param>
        /// <param name="filteredFiles"></param>
        /// <returns>
        /// A list of full paths to all downloaded artifacts.
        /// </returns>
        public List<string> DownloadFiltered(string directory = null, List<string> filteredFiles = null,
            bool flatten = false, bool overwrite = true)
        {
            if (directory == null)
                directory = Directory.GetCurrentDirectory();
            var downloaded = new List<string>();
            foreach (var url in m_urls)
            {
                if (filteredFiles != null)
                {
                    foreach (var filteredFile in filteredFiles)
                    {
                        var currentFilename = new Wildcard(GetFilename(filteredFile), RegexOptions.IgnoreCase);
                        var currentExt = new Wildcard(GetExtension(filteredFile), RegexOptions.IgnoreCase);

                        // user probably didnt use to artifact url generating functions
                        Debug.Assert(url.StartsWith("/repository/download/"));

                        // figure out local filename
                        var parts = url.Split('/').Skip(5).ToArray();
                        var destination = flatten
                            ? parts.Last()
                            : string.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), parts);
                        destination = Path.Combine(directory, destination);


                        if (currentFilename.IsMatch(Path.GetFileNameWithoutExtension(destination)) &&
                            currentExt.IsMatch(Path.GetExtension(destination)))
                        {
                            // create directories that doesnt exist
                            var directoryName = Path.GetDirectoryName(destination);
                            if (directoryName != null && !Directory.Exists(directoryName))
                                Directory.CreateDirectory(directoryName);

                            downloaded.Add(Path.GetFullPath(destination));

                            // if the file already exists delete it or move to next artifact
                            if (File.Exists(destination))
                            {
                                if (overwrite) File.Delete(destination);
                                else continue;
                            }

                            var currentUrl = url;
                            if (!string.IsNullOrEmpty(m_param))
                            {
                                currentUrl = $"{currentUrl}?{m_param}";
                            }

                            m_caller.GetDownloadFormat(tempFile => File.Move(tempFile, destination), currentUrl, false);
                            break;
                        }
                    }
                }
            }

            return downloaded;
        }

        public async Task<List<string>> DownloadFilteredAsync(string directory = null, List<string> filteredFiles = null,
            bool flatten = false, bool overwrite = true)
        {
            if (directory == null)
                directory = Directory.GetCurrentDirectory();
            var downloaded = new List<string>();
            foreach (var url in m_urls)
            {
                if (filteredFiles != null)
                {
                    foreach (var filteredFile in filteredFiles)
                    {
                        var currentFilename = new Wildcard(GetFilename(filteredFile), RegexOptions.IgnoreCase);
                        var currentExt = new Wildcard(GetExtension(filteredFile), RegexOptions.IgnoreCase);

                        // user probably didnt use to artifact url generating functions
                        Debug.Assert(url.StartsWith("/repository/download/"));

                        // figure out local filename
                        var parts = url.Split('/').Skip(5).ToArray();
                        var destination = flatten
                            ? parts.Last()
                            : string.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), parts);
                        destination = Path.Combine(directory, destination);


                        if (currentFilename.IsMatch(Path.GetFileNameWithoutExtension(destination)) &&
                            currentExt.IsMatch(Path.GetExtension(destination)))
                        {
                            // create directories that doesnt exist
                            var directoryName = Path.GetDirectoryName(destination);
                            if (directoryName != null && !Directory.Exists(directoryName))
                                Directory.CreateDirectory(directoryName);

                            downloaded.Add(Path.GetFullPath(destination));

                            // if the file already exists delete it or move to next artifact
                            if (File.Exists(destination))
                            {
                                if (overwrite) File.Delete(destination);
                                else continue;
                            }

                            var currentUrl = url;
                            if (!string.IsNullOrEmpty(m_param))
                            {
                                currentUrl = $"{currentUrl}?{m_param}";
                            }

                            await m_caller.GetDownloadFormatAsync(tempFile => File.Move(tempFile, destination), currentUrl,
                                false);
                            break;
                        }
                    }
                }
            }

            return downloaded;
        }

        private static string GetExtension(string path)
        {
            return path.Substring(path.LastIndexOf('.'));
        }

        private static string GetFilename(string path)
        {
            return path.Substring(0, path.LastIndexOf('.'));
        }
    }

    internal class Wildcard : Regex
    {
        /// <summary>
        /// Initializes a wildcard with the given search pattern.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to match.</param>
        public Wildcard(string pattern)
            : base(WildcardToRegex(pattern))
        {
        }

        /// <summary>
        /// Initializes a wildcard with the given search pattern and options.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to match.</param>
        /// <param name="options">A combination of one or more
        /// <see cref="RegexOptions"/>.</param>
        public Wildcard(string pattern, RegexOptions options)
            : base(WildcardToRegex(pattern), options)
        {
        }

        /// <summary>
        /// Converts a wildcard to a regex.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to convert.</param>
        /// <returns>A regex equivalent of the given wildcard.</returns>
        public static string WildcardToRegex(string pattern)
        {
            return "^" + Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
        }
    }
}