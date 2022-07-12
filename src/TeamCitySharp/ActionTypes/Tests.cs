using System.Collections.Generic;
using System.Linq;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public class Tests : ITests
    {
        #region Attributes

        private ITeamCityCaller m_caller;

        #endregion

        #region Constructor

        internal Tests(ITeamCityCaller caller)
        {
            m_caller = caller;
        }

        #endregion

        #region Public Methods

        public TestOccurrences ByBuildLocator(BuildLocator locator)
        {
            return m_caller.Get<TestOccurrences>($"/testOccurrences?locator=build:({locator})");
        }

        public async Task<TestOccurrences> ByBuildLocatorAsync(BuildLocator locator)
        {
            return await m_caller.GetAsync<TestOccurrences>($"/testOccurrences?locator=build:({locator})");
        }

        public TestOccurrences ByProjectLocator(ProjectLocator locator)
        {
            return m_caller.Get<TestOccurrences>($"/testOccurrences?locator=currentlyFailing:true,affectedProject:({locator})");
        }

        public async Task<TestOccurrences> ByProjectLocatorAsync(ProjectLocator locator)
        {
            return await m_caller.GetAsync<TestOccurrences>($"/testOccurrences?locator=currentlyFailing:true,affectedProject:({locator})");
        }

        public TestOccurrences ByTestLocator(TestLocator locator)
        {
            return m_caller.Get<TestOccurrences>($"/testOccurrences?locator=test:({locator})");
        }

        public async Task<TestOccurrences> ByTestLocatorAsync(TestLocator locator)
        {
            return await m_caller.GetAsync<TestOccurrences>($"/testOccurrences?locator=test:({locator})");
        }

        public List<TestOccurrences> All(BuildLocator locator)
        {
            return AllResults(ByBuildLocator(locator));
        }

        public async Task<List<TestOccurrences>> AllAsync(BuildLocator locator)
        {
            return await AllResultsAsync(await ByBuildLocatorAsync(locator));
        }

        public List<TestOccurrences> All(ProjectLocator locator)
        {
            return AllResults(ByProjectLocator(locator));
        }

        public async Task<List<TestOccurrences>> AllAsync(ProjectLocator locator)
        {
            return await AllResultsAsync(await ByProjectLocatorAsync(locator));
        }

        public List<TestOccurrences> All(TestLocator locator)
        {
            return AllResults(ByTestLocator(locator));
        }

        public async Task<List<TestOccurrences>> AllAsync(TestLocator locator)
        {
            return await AllResultsAsync(await ByTestLocatorAsync(locator));
        }

        #endregion

        #region Private Method

        private List<TestOccurrences> AllResults(TestOccurrences firstPageResult)
        {
            var result = new List<TestOccurrences>() {firstPageResult};
            while (!(string.IsNullOrEmpty(result.Last().NextHref)))
            {
                var response = m_caller.GetNextHref<TestOccurrences>(result.Last().NextHref);
                result.Add(response);
            }

            return result;
        }

        private async Task<List<TestOccurrences>> AllResultsAsync(TestOccurrences firstPageResult)
        {
            var result = new List<TestOccurrences>() {firstPageResult};
            while (!string.IsNullOrEmpty(result.Last().NextHref))
            {
                var response = await m_caller.GetNextHrefAsync<TestOccurrences>(result.Last().NextHref);
                result.Add(response);
            }

            return result;
        }

        #endregion
    }
}