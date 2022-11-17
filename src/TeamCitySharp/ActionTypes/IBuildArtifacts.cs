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
        /// Get Artifacts. Use <see cref="DownloadArtifactAsync(ArtifactFile)"/> to download a <see cref="ArtifactFile"/>.
        /// </summary>
        /// <param name="buildId"></param>
        /// <param name="subPath"></param>
        /// <returns></returns>
        Task<ArtifactFiles> GetArtifactsAsync(string buildId, string subPath = "");
        Task<ArtifactFiles> GetArtifactsByLocatorAsync(BuildLocator locator, string subPath = "");

        Task<byte[]> DownloadArtifactAsync(string href);
        Task<byte[]> DownloadArtifactAsync(ArtifactFile file);
    }
}