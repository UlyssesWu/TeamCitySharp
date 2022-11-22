using System;
using System.Threading.Tasks;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace TeamCitySharp.ActionTypes
{
    public interface IBuildArtifacts : IBuildArtifactsAsync
    {
        void DownloadArtifactsByBuildId(string buildId, Action<string> downloadHandler);

        ArtifactWrapper ByBuildConfigId(string buildConfigId, string param = "");
    }

    public interface IBuildArtifactsAsync
    {
        /// <summary>
        /// Download ALL artifacts as a zip file
        /// </summary>
        /// <param name="buildId"></param>
        /// <param name="downloadHandler"></param>
        /// <returns></returns>
        Task DownloadArtifactsByBuildIdAsync(string buildId, Action<string> downloadHandler);

        //see: https://www.jetbrains.com/help/teamcity/rest/manage-finished-builds.html#Get+Build+Artifacts
        /// <summary>
        /// Get a build's Artifacts info. Use <see cref="DownloadArtifactAsync(ArtifactItem)"/> to download a <see cref="ArtifactItem"/>.
        /// </summary>
        /// <param name="buildId"></param>
        /// <param name="subPath"></param>
        /// <returns></returns>
        Task<ArtifactFiles> GetArtifactsAsync(string buildId, string subPath = "");
        /// <summary>
        /// Get a build's Artifacts info.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="subPath"></param>
        /// <returns></returns>
        Task<ArtifactFiles> GetArtifactsByLocatorAsync(BuildLocator locator, string subPath = "");

        /// <summary>
        /// Get Artifacts info. <paramref name="item"/> usually is a <see cref="ArtifactItem.Children"/>
        /// </summary>
        /// <param name="item"><see cref="ArtifactItem.Children"/></param>
        /// <returns></returns>
        Task<ArtifactFiles> GetArtifactsAsync(ArtifactItem item);

        /// <summary>
        /// Download a single artifact
        /// </summary>
        /// <param name="href"><see cref="ArtifactContent.Href"/> from <see cref="ArtifactItem.Content"/></param>
        /// <returns></returns>
        Task<byte[]> DownloadArtifactAsync(string href);
        /// <summary>
        /// Download a single artifact
        /// </summary>
        Task<byte[]> DownloadArtifactAsync(ArtifactItem item);
    }
}