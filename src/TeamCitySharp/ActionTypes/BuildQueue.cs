using System.Collections.Generic;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public class BuildQueue : IBuildQueue
    {
        private readonly ITeamCityCaller m_caller;
        private string m_fields;

        internal BuildQueue(ITeamCityCaller caller)
        {
            m_caller = caller;
        }

        public BuildQueue GetFields(string fields)
        {
            var newInstance = (BuildQueue)MemberwiseClone();
            newInstance.m_fields = fields;
            return newInstance;
        }

        public List<Build> All()
        {
            var buildQueue =
                m_caller.Get<BuildWrapper>(ActionHelper.CreateFieldUrl("/buildQueue", m_fields));

            return buildQueue.Build;
        }

        public async Task<List<Build>> AllAsync()
        {
            var buildQueue =
                await m_caller.GetAsync<BuildWrapper>(ActionHelper.CreateFieldUrl("/buildQueue", m_fields));

            return buildQueue.Build;
        }

        public List<Build> ByBuildTypeLocator(BuildTypeLocator locator)
        {
            var buildWrapper = m_caller.Get<BuildWrapper>($"/buildQueue?locator=buildType:({locator})");
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        public async Task<List<Build>> ByBuildTypeLocatorAsync(BuildTypeLocator locator)
        {
            var buildWrapper = await m_caller.GetAsync<BuildWrapper>($"/buildQueue?locator=buildType:({locator})");
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        public List<Build> ByProjectLocator(ProjectLocator locator)
        {
            var buildWrapper = m_caller.Get<BuildWrapper>($"/buildQueue?locator=project:({locator})");
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }

        public async Task<List<Build>> ByProjectLocatorAsync(ProjectLocator locator)
        {
            var buildWrapper = await m_caller.GetAsync<BuildWrapper>($"/buildQueue?locator=project:({locator})");
            return int.Parse(buildWrapper.Count) > 0 ? buildWrapper.Build : new List<Build>();
        }
    }
}