using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace TeamCitySharp.ActionTypes
{
    public interface IBuilds : IBuildsAsync
    {
        Build ById(string id);
        Build LastBuildByAgent(string agentName, List<string> param = null);
        Build LastBuildByBuildConfigId(string buildConfigId, List<string> param = null);
        Build LastErrorBuildByBuildConfigId(string buildConfigId, List<string> param = null);
        Build LastFailedBuildByBuildConfigId(string buildConfigId, List<string> param = null);
        Build LastSuccessfulBuildByBuildConfigId(string buildConfigId, List<string> param = null);
        Builds GetFields(string fields);
        List<Build> AffectedProject(string projectId, long count = 100, List<string> param = null);
        List<Build> AllBuildsOfStatusSinceDate(DateTime date, BuildStatus buildStatus);
        List<Build> AllRunningBuild();
        List<Build> AllSinceDate(DateTime date, long count = 100, List<string> param = null);
        List<Build> ByBranch(string branchName);
        List<Build> ByBuildConfigId(string buildConfigId);
        List<Build> ByBuildConfigId(string buildConfigId, List<string> param);
        List<Build> ByBuildLocator(BuildLocator locator);
        List<Build> ByBuildLocator(BuildLocator locator, List<string> param);
        List<Build> ByConfigIdAndTag(string buildConfigId, string tag);
        List<Build> ByUserName(string userName);
        List<Build> ErrorBuildsByBuildConfigId(string buildConfigId, List<string> param = null);
        List<Build> FailedBuildsByBuildConfigId(string buildConfigId, List<string> param = null);
        List<Build> NextBuilds(string buildId, long count = 100, List<string> param = null);
        List<Build> NonSuccessfulBuildsForUser(string userName);
        List<Build> RetrieveEntireBuildChainFrom(string buildConfigId, bool includeInitial = true, List<string> param = null);
        List<Build> RetrieveEntireBuildChainTo(string buildConfigId, bool includeInitial = true, List<string> param = null);
        List<Build> RunningByBuildConfigId(string buildConfigId);
        List<Build> SuccessfulBuildsByBuildConfigId(string buildConfigId, List<string> param = null);
        void Add2QueueBuildByBuildConfigId(string buildConfigId);
        void DownloadLogs(string projectId, bool zipped, Action<string> downloadHandler);
        void PinBuildByBuildNumber(string buildConfigId, string buildNumber, string comment);
        void UnPinBuildByBuildNumber(string buildConfigId, string buildNumber);
    }

    public interface IBuildsAsync
    {
        Task<Build> ByIdAsync(string id);
        Task<Build> LastBuildByAgentAsync(string agentName, List<string> param = null);
        Task<Build> LastBuildByBuildConfigIdAsync(string buildConfigId, List<string> param = null);
        Task<Build> LastErrorBuildByBuildConfigIdAsync(string buildConfigId, List<string> param = null);
        Task<Build> LastFailedBuildByBuildConfigIdAsync(string buildConfigId, List<string> param = null);
        Task<Build> LastSuccessfulBuildByBuildConfigIdAsync(string buildConfigId, List<string> param = null);
        Task<List<Build>> AffectedProjectAsync(string projectId, long count = 100, List<string> param = null);
        Task<List<Build>> AllBuildsOfStatusSinceDateAsync(DateTime date, BuildStatus buildStatus);
        Task<List<Build>> AllRunningBuildAsync();
        Task<List<Build>> AllSinceDateAsync(DateTime date, long count = 100, List<string> param = null);
        Task<List<Build>> ByBranchAsync(string branchName);
        Task<List<Build>> ByBuildConfigIdAsync(string buildConfigId);
        Task<List<Build>> ByBuildConfigIdAsync(string buildConfigId, List<string> param);
        Task<List<Build>> ByBuildLocatorAsync(BuildLocator locator);
        Task<List<Build>> ByBuildLocatorAsync(BuildLocator locator, List<string> param);
        Task<List<Build>> ByConfigIdAndTagAsync(string buildConfigId, string tag);
        Task<List<Build>> ByUserNameAsync(string userName);
        Task<List<Build>> ErrorBuildsByBuildConfigIdAsync(string buildConfigId, List<string> param = null);
        Task<List<Build>> FailedBuildsByBuildConfigIdAsync(string buildConfigId, List<string> param = null);
        Task<List<Build>> NextBuildsAsync(string buildId, long count = 100, List<string> param = null);
        Task<List<Build>> NonSuccessfulBuildsForUserAsync(string userName);
        Task<List<Build>> RetrieveEntireBuildChainFromAsync(string buildConfigId, bool includeInitial = true, List<string> param = null);
        Task<List<Build>> RetrieveEntireBuildChainToAsync(string buildConfigId, bool includeInitial = true, List<string> param = null);
        Task<List<Build>> RunningByBuildConfigIdAsync(string buildConfigId);
        Task<List<Build>> SuccessfulBuildsByBuildConfigIdAsync(string buildConfigId, List<string> param = null);
        Task Add2QueueBuildByBuildConfigIdAsync(string buildConfigId);
        Task DownloadLogsAsync(string projectId, bool zipped, Action<string> downloadHandler);
        Task PinBuildByBuildNumberAsync(string buildConfigId, string buildNumber, string comment);
        Task UnPinBuildByBuildNumberAsync(string buildConfigId, string buildNumber);
    }
}