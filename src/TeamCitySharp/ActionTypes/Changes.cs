using System.Collections.Generic;
using System.Linq;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    internal class Changes : IChanges
    {
        private readonly ITeamCityCaller m_caller;
        private string m_fields;

        internal Changes(ITeamCityCaller caller)
        {
            m_caller = caller;
        }

        public Changes GetFields(string fields)
        {
            var newInstance = (Changes)MemberwiseClone();
            newInstance.m_fields = fields;
            return newInstance;
        }

        public List<Change> All()
        {
            var changeWrapper = m_caller.Get<ChangeWrapper>(ActionHelper.CreateFieldUrl("/changes", m_fields));

            return changeWrapper.Change;
        }

        public async Task<List<Change>> AllAsync()
        {
            var changeWrapper = await m_caller.GetAsync<ChangeWrapper>(ActionHelper.CreateFieldUrl("/changes", m_fields));

            return changeWrapper.Change;
        }

        public Change ByChangeId(string id)
        {
            var change = m_caller.GetFormat<Change>(ActionHelper.CreateFieldUrl("/changes/id:{0}", m_fields), id);

            return change;
        }

        public async Task<Change> ByChangeIdAsync(string id)
        {
            var change = await m_caller.GetFormatAsync<Change>(ActionHelper.CreateFieldUrl("/changes/id:{0}", m_fields), id);

            return change;
        }

        public List<Change> ByBuildConfigId(string buildConfigId)
        {
            var changeWrapper =
                m_caller.GetFormat<ChangeWrapper>(ActionHelper.CreateFieldUrl("/changes?buildType={0}", m_fields),
                    buildConfigId);

            return changeWrapper.Change;
        }

        public async Task<List<Change>> ByBuildConfigIdAsync(string buildConfigId)
        {
            var changeWrapper =
                await m_caller.GetFormatAsync<ChangeWrapper>(ActionHelper.CreateFieldUrl("/changes?buildType={0}", m_fields),
                    buildConfigId);

            return changeWrapper.Change;
        }

        public Change LastChangeDetailByBuildConfigId(string buildConfigId)
        {
            var changes = ByBuildConfigId(buildConfigId);

            return changes.FirstOrDefault();
        }

        public async Task<Change> LastChangeDetailByBuildConfigIdAsync(string buildConfigId)
        {
            var changes = await ByBuildConfigIdAsync(buildConfigId);

            return changes.FirstOrDefault();
        }
    }
}