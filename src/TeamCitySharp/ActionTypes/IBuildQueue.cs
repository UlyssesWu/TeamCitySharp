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
    }
}