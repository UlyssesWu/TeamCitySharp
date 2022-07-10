using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace TeamCitySharp.ActionTypes
{
    public interface ITests
#if ENABLE_ASYNC
        : ITestsAsync
#endif
    {
        TestOccurrences ByBuildLocator(BuildLocator locator);
        TestOccurrences ByProjectLocator(ProjectLocator locator);
        TestOccurrences ByTestLocator(TestLocator locator);
        List<TestOccurrences> All(BuildLocator locator);
        List<TestOccurrences> All(ProjectLocator locator);
        List<TestOccurrences> All(TestLocator locator);
    }

    public interface ITestsAsync
    {
        Task<TestOccurrences> ByBuildLocatorAsync(BuildLocator locator);
        Task<TestOccurrences> ByProjectLocatorAsync(ProjectLocator locator);
        Task<TestOccurrences> ByTestLocatorAsync(TestLocator locator);
        Task<List<TestOccurrences>> AllAsync(BuildLocator locator);
        Task<List<TestOccurrences>> AllAsync(ProjectLocator locator);
        Task<List<TestOccurrences>> AllAsync(TestLocator locator);
    }
}