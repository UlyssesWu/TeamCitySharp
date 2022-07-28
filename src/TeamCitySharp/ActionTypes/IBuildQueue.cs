using System.Collections.Generic;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public interface IBuildQueue : IBuildQueueAsync
    {
        List<Build> All();

        BuildQueue GetFields(string fields);
        List<Build> ByBuildTypeLocator(BuildTypeLocator locator);

        List<Build> ByProjectLocator(ProjectLocator projectLocator);
    }

    public interface IBuildQueueAsync
    {
        Task<List<Build>> AllAsync();
        Task<List<Build>> ByBuildTypeLocatorAsync(BuildTypeLocator locator);
        Task<List<Build>> ByProjectLocatorAsync(ProjectLocator projectLocator);

        /// <summary>
        /// Cancel a queued build.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="comment">to provide an optional comment on why the build was canceled</param>
        /// <param name="reAddIntoQueue">a boolean property which declares if this is a request to restore a previously canceled build</param>
        /// <returns></returns>
        Task<Build> CancelQueuedBuildAsync(BuildLocator locator, string comment = "", bool reAddIntoQueue = false);

        /// <inheritdoc cref="CancelQueuedBuildAsync"/>
        Task<Build> CancelQueuedBuildByIdAsync(string buildId, string comment = "", bool reAddIntoQueue = false);
    }
}