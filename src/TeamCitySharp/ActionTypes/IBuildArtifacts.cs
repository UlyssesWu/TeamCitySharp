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
        Task DownloadArtifactsByBuildIdAsync(string buildId, Action<string> downloadHandler);

        //see: https://www.jetbrains.com/help/teamcity/rest/manage-finished-builds.html#Get+Build+Artifacts
        Task<ArtifactFiles> GetArtifactsAsync(string buildId, string subPath = "");
        Task<ArtifactFiles> GetArtifactsByLocatorAsync(BuildLocator locator, string subPath = "");
    }
}