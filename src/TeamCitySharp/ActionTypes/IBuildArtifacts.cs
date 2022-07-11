using System;
using System.Threading.Tasks;

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
    }
}