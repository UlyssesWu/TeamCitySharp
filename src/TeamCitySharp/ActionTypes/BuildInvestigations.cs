﻿using System.Collections.Generic;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public class BuildInvestigations : IBuildInvestigations
    {
        private readonly ITeamCityCaller m_caller;
        private string m_fields;

        internal BuildInvestigations(ITeamCityCaller caller)
        {
            m_caller = caller;
        }

        public BuildInvestigations GetFields(string fields)
        {
            var newInstance = (BuildInvestigations)MemberwiseClone();
            newInstance.m_fields = fields;
            return newInstance;
        }

        #region IBuildInvestigations Members

        public List<Investigation> All()
        {
            var url = ActionHelper.CreateFieldUrl("/investigations", m_fields);

            var wrapper = m_caller.Get<InvestigationWrapper>(url);

            return wrapper.Investigation;
        }

        public async Task<List<Investigation>> AllAsync()
        {
            var url = ActionHelper.CreateFieldUrl("/investigations", m_fields);

            var wrapper = await m_caller.GetAsync<InvestigationWrapper>(url);

            return wrapper.Investigation;
        }

        public List<Investigation> InvestigationsByBuildTypeId(string buildTypeId)
        {
            var investigationsByBuildTypeId = new List<Investigation>();
            var investigations = All();

            foreach (var investigation in investigations)
            {
                if (investigation.Scope?.BuildTypes != null)
                {
                    foreach (var buildType in investigation.Scope.BuildTypes.BuildType)
                    {
                        if (buildType.Id.Equals(buildTypeId))
                            investigationsByBuildTypeId.Add(investigation);
                    }
                }
            }

            return investigationsByBuildTypeId;
        }

        public async Task<List<Investigation>> InvestigationsByBuildTypeIdAsync(string buildTypeId)
        {
            var investigationsByBuildTypeId = new List<Investigation>();
            var investigations = await AllAsync();

            foreach (var investigation in investigations)
            {
                if (investigation.Scope?.BuildTypes != null)
                {
                    foreach (var buildType in investigation.Scope.BuildTypes.BuildType)
                    {
                        if (buildType.Id.Equals(buildTypeId))
                            investigationsByBuildTypeId.Add(investigation);
                    }
                }
            }

            return investigationsByBuildTypeId;
        }

        #endregion
    }
}