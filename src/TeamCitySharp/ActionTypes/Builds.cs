﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace TeamCitySharp.ActionTypes
{
    public class Builds : IBuilds
    {
        #region Attributes

        private readonly ITeamCityCaller m_caller;
        private string m_fields;

        #endregion

        #region Constructor

        internal Builds(ITeamCityCaller caller)
        {
            m_caller = caller;
        }

        #endregion

        #region Public Methods

        public Builds GetFields(string fields)
        {
            var newInstance = (Builds) MemberwiseClone();
            newInstance.m_fields = fields;
            return newInstance;
        }

        public List<Build> ByBuildLocator(BuildLocator locator)
        {
            var buildWrapper =
                m_caller.GetFormat<BuildWrapper>(ActionHelper.CreateFieldUrl("/builds?locator={0}", m_fields), locator);
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }
        
        public async Task<List<Build>> ByBuildLocatorAsync(BuildLocator locator)
        {
            var buildWrapper =
                await m_caller.GetFormatAsync<BuildWrapper>(ActionHelper.CreateFieldUrl("/builds?locator={0}", m_fields), locator);
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        public List<Build> ByBuildLocator(BuildLocator locator, List<string> param)
        {
            var strParam = GetParamLocator(param);
            var buildWrapper =
                m_caller.Get<BuildWrapper>(
                    ActionHelper.CreateFieldUrl($"/builds?locator={locator}{strParam}", m_fields));

            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }
        
        public async Task<List<Build>> ByBuildLocatorAsync(BuildLocator locator, List<string> param)
        {
            var strParam = GetParamLocator(param);
            var buildWrapper =
                await m_caller.GetAsync<BuildWrapper>(
                    ActionHelper.CreateFieldUrl($"/builds?locator={locator}{strParam}", m_fields));

            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        public Build LastBuildByAgent(string agentName, List<string> param = null)
        {
            return ByBuildLocator(BuildLocator.WithDimensions(agentName: agentName, maxResults: 1), param).SingleOrDefault();
        }

        public async Task<Build> LastBuildByAgentAsync(string agentName, List<string> param = null)
        {
            return (await ByBuildLocatorAsync(BuildLocator.WithDimensions(agentName: agentName, maxResults: 1), param)).SingleOrDefault();
        }

        public void Add2QueueBuildByBuildConfigId(string buildConfigId)
        {
            m_caller.GetFormat("/action.html?add2Queue={0}", buildConfigId);
        }

        public async Task Add2QueueBuildByBuildConfigIdAsync(string buildConfigId)
        {
            await m_caller.GetFormatAsync("/action.html?add2Queue={0}", buildConfigId);
        }

        public List<Build> SuccessfulBuildsByBuildConfigId(string buildConfigId, List<string> param = null)
        {
            return ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                status: BuildStatus.SUCCESS
            ), param);
        }

        public async Task<List<Build>> SuccessfulBuildsByBuildConfigIdAsync(string buildConfigId, List<string> param = null)
        {
            return await ByBuildLocatorAsync(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                status: BuildStatus.SUCCESS
            ), param);
        }


        public Build LastSuccessfulBuildByBuildConfigId(string buildConfigId, List<string> param = null)
        {
            var builds = ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                status: BuildStatus.SUCCESS,
                maxResults: 1
            ), param);
            return builds != null ? builds.FirstOrDefault() : new Build();
        }

        public async Task<Build> LastSuccessfulBuildByBuildConfigIdAsync(string buildConfigId, List<string> param = null)
        {
            var builds = await ByBuildLocatorAsync(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                status: BuildStatus.SUCCESS,
                maxResults: 1
            ), param);
            return builds != null ? builds.FirstOrDefault() : new Build();
        }

        public List<Build> FailedBuildsByBuildConfigId(string buildConfigId, List<string> param = null)
        {
            return ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                status: BuildStatus.FAILURE
            ), param);
        }

        public async Task<List<Build>> FailedBuildsByBuildConfigIdAsync(string buildConfigId, List<string> param = null)
        {
            return await ByBuildLocatorAsync(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                status: BuildStatus.FAILURE
            ), param);
        }

        public Build LastFailedBuildByBuildConfigId(string buildConfigId, List<string> param = null)
        {
            var builds = ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                status: BuildStatus.FAILURE,
                maxResults: 1
            ), param);
            return builds != null ? builds.FirstOrDefault() : new Build();
        }

        public async Task<Build> LastFailedBuildByBuildConfigIdAsync(string buildConfigId, List<string> param = null)
        {
            var builds = await ByBuildLocatorAsync(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                status: BuildStatus.FAILURE,
                maxResults: 1
            ), param);
            return builds != null ? builds.FirstOrDefault() : new Build();
        }

        public Build LastBuildByBuildConfigId(string buildConfigId, List<string> param = null)
        {
            var builds = ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                maxResults: 1
            ), param);
            return builds != null ? builds.FirstOrDefault() : new Build();
        }

        public async Task<Build> LastBuildByBuildConfigIdAsync(string buildConfigId, List<string> param = null)
        {
            var builds = await ByBuildLocatorAsync(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                maxResults: 1
            ), param);
            return builds != null ? builds.FirstOrDefault() : new Build();
        }

        public List<Build> ErrorBuildsByBuildConfigId(string buildConfigId, List<string> param = null)
        {
            return ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                status: BuildStatus.ERROR
            ), param);
        }

        public async Task<List<Build>> ErrorBuildsByBuildConfigIdAsync(string buildConfigId, List<string> param = null)
        {
            return await ByBuildLocatorAsync(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                status: BuildStatus.ERROR
            ), param);
        }

        public Build LastErrorBuildByBuildConfigId(string buildConfigId, List<string> param = null)
        {
            var builds = ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                status: BuildStatus.ERROR,
                maxResults: 1
            ), param);
            return builds != null ? builds.FirstOrDefault() : new Build();
        }

        public async Task<Build> LastErrorBuildByBuildConfigIdAsync(string buildConfigId, List<string> param = null)
        {
            var builds = await ByBuildLocatorAsync(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                status: BuildStatus.ERROR,
                maxResults: 1
            ), param);
            return builds != null ? builds.FirstOrDefault() : new Build();
        }

        public List<Build> ByBuildConfigId(string buildConfigId, List<string> param)
        {
            return ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId)), param);
        }

        public async Task<List<Build>> ByBuildConfigIdAsync(string buildConfigId, List<string> param)
        {
            return await ByBuildLocatorAsync(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId)), param);
        }

        public Build ById(string id)
        {
            var build = m_caller.GetFormat<Build>(ActionHelper.CreateFieldUrl("/builds/id:{0}", m_fields), id);

            return build ?? new Build();
        }

        public async Task<Build> ByIdAsync(string id)
        {
            var build = await m_caller.GetFormatAsync<Build>(ActionHelper.CreateFieldUrl("/builds/id:{0}", m_fields), id);

            return build ?? new Build();
        }

        public List<Build> ByBuildConfigId(string buildConfigId)
        {
            return ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId)
            ));
        }

        public async Task<List<Build>> ByBuildConfigIdAsync(string buildConfigId)
        {
            return await ByBuildLocatorAsync(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId)
            ));
        }

        public List<Build> RunningByBuildConfigId(string buildConfigId)
        {
            return ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId)),
                new List<string> {"running:true"});
        }

        public async Task<List<Build>> RunningByBuildConfigIdAsync(string buildConfigId)
        {
            return await ByBuildLocatorAsync(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId)),
                new List<string> { "running:true" });
        }

        public List<Build> ByConfigIdAndTag(string buildConfigId, string tag)
        {
            return ByConfigIdAndTag(buildConfigId, new[] {tag});
        }

        public async Task<List<Build>> ByConfigIdAndTagAsync(string buildConfigId, string tag)
        {
            return await ByConfigIdAndTagAsync(buildConfigId, new[] { tag });
        }

        public List<Build> ByConfigIdAndTag(string buildConfigId, string[] tags)
        {
            return ByBuildLocator(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                tags: tags
            ));
        }

        public async Task<List<Build>> ByConfigIdAndTagAsync(string buildConfigId, string[] tags)
        {
            return await ByBuildLocatorAsync(BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfigId),
                tags: tags
            ));
        }
        
        public List<Build> ByUserName(string userName)
        {
            return ByBuildLocator(BuildLocator.WithDimensions(
                user: UserLocator.WithUserName(userName)
            ));
        }

        public async Task<List<Build>> ByUserNameAsync(string userName)
        {
            return await ByBuildLocatorAsync(BuildLocator.WithDimensions(
                user: UserLocator.WithUserName(userName)
            ));
        }

        public List<Build> AllSinceDate(DateTime date, long count = 100, List<string> param = null)
        {
            if (param == null)
            {
                param = new List<string> {"defaultFilter:false"};
            }

            param.Add($"count({count})");

            return ByBuildLocator(BuildLocator.WithDimensions(sinceDate: date), param);
        }

        public async Task<List<Build>> AllSinceDateAsync(DateTime date, long count = 100, List<string> param = null)
        {
            param ??= new List<string> { "defaultFilter:false" };

            param.Add($"count({count})");

            return await ByBuildLocatorAsync(BuildLocator.WithDimensions(sinceDate: date), param);
        }

        public List<Build> AllRunningBuild()
        {
            var buildWrapper =
                m_caller.GetFormat<BuildWrapper>(ActionHelper.CreateFieldUrl("/builds?locator=running:true", m_fields));
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        public async Task<List<Build>> AllRunningBuildAsync()
        {
            var buildWrapper =
                await m_caller.GetFormatAsync<BuildWrapper>(ActionHelper.CreateFieldUrl("/builds?locator=running:true", m_fields));
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        public List<Build> ByBranch(string branchName)
        {
            return ByBuildLocator(BuildLocator.WithDimensions(branch: branchName));
        }

        public async Task<List<Build>> ByBranchAsync(string branchName)
        {
            return await ByBuildLocatorAsync(BuildLocator.WithDimensions(branch: branchName));
        }

        public List<Build> AllBuildsOfStatusSinceDate(DateTime date, BuildStatus buildStatus)
        {
            return ByBuildLocator(BuildLocator.WithDimensions(sinceDate: date, status: buildStatus));
        }

        public async Task<List<Build>> AllBuildsOfStatusSinceDateAsync(DateTime date, BuildStatus buildStatus)
        {
            return await ByBuildLocatorAsync(BuildLocator.WithDimensions(sinceDate: date, status: buildStatus));
        }

        public List<Build> NonSuccessfulBuildsForUser(string userName)
        {
            var builds = ByUserName(userName);

            return builds?.Where(b => b.Status != "SUCCESS").ToList();
        }

        public async Task<List<Build>> NonSuccessfulBuildsForUserAsync(string userName)
        {
            var builds = await ByUserNameAsync(userName);

            return builds?.Where(b => b.Status != "SUCCESS").ToList();
        }

        public List<Build> RetrieveEntireBuildChainFrom(string buildId, bool includeInitial = true, List<string> param = null)
        {
            var strIncludeInitial = includeInitial ? "true" : "false";
            if (param == null)
            {
                param = new List<string> {"defaultFilter:false"};
            }

            return GetBuildListQuery("/builds?locator=snapshotDependency:(from:(id:{0}),includeInitial:" + strIncludeInitial + "){1}",
                buildId, param);
        }

        public async Task<List<Build>> RetrieveEntireBuildChainFromAsync(string buildId, bool includeInitial = true, List<string> param = null)
        {
            var strIncludeInitial = includeInitial ? "true" : "false";
            param ??= new List<string> { "defaultFilter:false" };

            return await GetBuildListQueryAsync("/builds?locator=snapshotDependency:(from:(id:{0}),includeInitial:" + strIncludeInitial + "){1}",
                buildId, param);
        }

        public List<Build> RetrieveEntireBuildChainTo(string buildId, bool includeInitial = true, List<string> param = null)
        {
            var strIncludeInitial = includeInitial ? "true" : "false";
            if (param == null)
            {
                param = new List<string> {"defaultFilter:false"};
            }

            return GetBuildListQuery("/builds?locator=snapshotDependency:(to:(id:{0}),includeInitial:" + strIncludeInitial + "){1}",
                buildId, param);
        }

        public async Task<List<Build>> RetrieveEntireBuildChainToAsync(string buildId, bool includeInitial = true, List<string> param = null)
        {
            var strIncludeInitial = includeInitial ? "true" : "false";
            param ??= new List<string> { "defaultFilter:false" };

            return await GetBuildListQueryAsync("/builds?locator=snapshotDependency:(to:(id:{0}),includeInitial:" + strIncludeInitial + "){1}",
                buildId, param);
        }

        /// <summary>
        /// Retrieves the list of build after a build id. 
        /// 
        /// IMPORTANT NOTE: The list starts from the latest build to oldest  (Descending)
        /// </summary>
        /// <param name="buildId"></param>
        /// <param name="count"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Build> NextBuilds(string buildId, long count = 100, List<string> param = null)
        {
            return GetBuildListQuery("/builds?locator=sinceBuild:(id:{0}),count(" + count + "){1}",
                buildId, param);
        }

        public async Task<List<Build>> NextBuildsAsync(string buildId, long count = 100, List<string> param = null)
        {
            param ??= new List<string> { "defaultFilter:false" };
            param.Add($"count({count})");
            return await GetBuildListQueryAsync("/builds?locator=sinceBuild:(id:{0}),count(" + count + "){1}",
                buildId, param);
        }

        /// <summary>
        /// Retrieves the list of build affected by a project. 
        /// 
        /// IMPORTANT NOTE: The list starts from the latest build to oldest  (Descending)
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="count"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<Build> AffectedProject(string projectId, long count = 100, List<string> param = null)
        {
            return GetBuildListQuery("/builds?locator=affectedProject:(id:{0}),count(" + count + "){1}",
                projectId, param);
        }

        public async Task<List<Build>> AffectedProjectAsync(string projectId, long count = 100, List<string> param = null)
        {
            param ??= new List<string> { "defaultFilter:false" };
            param.Add($"count({count})");
            return await GetBuildListQueryAsync("/builds?locator=affectedProject:(id:{0}),count(" + count + "){1}",
                projectId, param);
        }

        /// <summary>
        /// Download log
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="zipped"></param>
        /// <param name="downloadHandler"></param>
        /// <returns></returns>
        public void DownloadLogs(string projectId, bool zipped, Action<string> downloadHandler)
        {
            var url = $"/downloadBuildLog.html?buildId={projectId}&archived={zipped}";
            m_caller.GetDownloadFormat(downloadHandler, url, false);
        }

        public async Task DownloadLogsAsync(string projectId, bool zipped, Action<string> downloadHandler)
        {
            var url = $"/downloadBuildLog.html?buildId={projectId}&archived={zipped}";
            await m_caller.GetDownloadFormatAsync(downloadHandler, url, false);
        }

        /// <summary>
        /// Pin a build by build number
        /// </summary>
        /// <param name="buildConfigId"></param>
        /// <param name="buildNumber"></param>
        /// <param name="comment"></param>
        public void PinBuildByBuildNumber(string buildConfigId, string buildNumber, string comment)
        {
            const string urlPart = "/builds/buildType:{0},number:{1}/pin/";
            m_caller.PutFormat(comment, HttpContentTypes.TextPlain, urlPart, buildConfigId, buildNumber);
        }

        public async Task PinBuildByBuildNumberAsync(string buildConfigId, string buildNumber, string comment)
        {
            const string urlPart = "/builds/buildType:{0},number:{1}/pin/";
            await m_caller.PutFormatAsync(comment, HttpContentTypes.TextPlain, urlPart, buildConfigId, buildNumber);
        }

        /// <summary>
        /// Unpin a build by build number
        /// </summary>
        /// <param name="buildConfigId"></param>
        /// <param name="buildNumber"></param>
        public void UnPinBuildByBuildNumber(string buildConfigId, string buildNumber)
        {
            var urlPart = $"/builds/buildType:{buildConfigId},number:{buildNumber}/pin/";
            m_caller.Delete(urlPart);
        }

        public async Task UnPinBuildByBuildNumberAsync(string buildConfigId, string buildNumber)
        {
            var urlPart = $"/builds/buildType:{buildConfigId},number:{buildNumber}/pin/";
            await m_caller.DeleteAsync(urlPart);
        }



        public async Task<Build> CancelBuildAsync(BuildLocator locator, string comment = "", bool reAddIntoQueue = false)
        {
            return await m_caller.PostFormatAsync<Build>(new BuildCancelRequest { Comment = comment, ReAddIntoQueue = reAddIntoQueue },
                HttpContentTypes.ApplicationJson, HttpContentTypes.ApplicationJson, $"/builds/{locator}");
        }

        public async Task SetBuildCommentAsync(BuildLocator locator, string comment = "")
        {
            await m_caller.PutAsync(comment, HttpContentTypes.TextPlain, $"/builds/{locator}/comment",
                HttpContentTypes.TextPlain);
        }

        public async Task DeleteBuildCommentAsync(BuildLocator locator)
        {
            await m_caller.DeleteAsync($"/builds/{locator}/comment");
        }

        public async Task<Comment> GetCanceledInfoAsync(BuildLocator locator)
        {
            return await m_caller.GetAsync<Comment>(ActionHelper.CreateFieldUrl($"/builds/{locator}/comment", m_fields));
        }

        public async Task<Build> CancelBuildByIdAsync(string buildId, string comment = "", bool reAddIntoQueue = false)
        {
            return await m_caller.PostFormatAsync<Build>(new BuildCancelRequest { Comment = comment, ReAddIntoQueue = reAddIntoQueue },
                HttpContentTypes.ApplicationJson, HttpContentTypes.ApplicationJson, $"/builds/id:{buildId}");
        }

        #endregion

        #region Private Methods

        private static string GetParamLocator(List<string> param)
        {
            var strParam = "";
            if (param != null)
            {
                foreach (var tmpParam in param)
                {
                    strParam += ",";
                    strParam += tmpParam;
                }
            }

            return strParam;
        }

        private List<Build> GetBuildListQuery(string url, string id, List<string> param = null)
        {
            var strParam = GetParamLocator(param);
            var buildWrapper =
                m_caller.GetFormat<BuildWrapper>(
                    ActionHelper.CreateFieldUrl(
                        url, m_fields),
                    id, strParam);
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        private async Task<List<Build>> GetBuildListQueryAsync(string url, string id, List<string> param = null)
        {
            var strParam = GetParamLocator(param);
            var buildWrapper =
                await m_caller.GetFormatAsync<BuildWrapper>(
                    ActionHelper.CreateFieldUrl(
                        url, m_fields),
                    id, strParam);
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        #endregion

        #region Async Methods

        #endregion
    }
}