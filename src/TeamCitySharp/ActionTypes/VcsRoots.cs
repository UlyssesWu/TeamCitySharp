using System.Collections.Generic;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public class VcsRoots : IVcsRoots
    {
        private readonly ITeamCityCaller m_caller;
        private string m_fields;

        internal VcsRoots(ITeamCityCaller caller)
        {
            m_caller = caller;
        }

        public VcsRoots GetFields(string fields)
        {
            var newInstance = (VcsRoots) MemberwiseClone();
            newInstance.m_fields = fields;
            return newInstance;
        }

        public List<VcsRoot> All()
        {
            var vcsRootWrapper = m_caller.Get<VcsRootWrapper>(ActionHelper.CreateFieldUrl("/vcs-roots", m_fields));

            return vcsRootWrapper.VcsRoot;
        }

        public async Task<List<VcsRoot>> AllAsync()
        {
            var vcsRootWrapper = await m_caller.GetAsync<VcsRootWrapper>(ActionHelper.CreateFieldUrl("/vcs-roots", m_fields));

            return vcsRootWrapper.VcsRoot;
        }

        public VcsRoot ById(string vcsRootId)
        {
            var vcsRoot = m_caller.GetFormat<VcsRoot>(ActionHelper.CreateFieldUrl("/vcs-roots/id:{0}", m_fields),
                vcsRootId);

            return vcsRoot;
        }

        public async Task<VcsRoot> ByIdAsync(string vcsRootId)
        {
            var vcsRoot = await m_caller.GetFormatAsync<VcsRoot>(ActionHelper.CreateFieldUrl("/vcs-roots/id:{0}", m_fields),
                vcsRootId);

            return vcsRoot;
        }

        public VcsRoot AttachVcsRoot(BuildTypeLocator locator, VcsRoot vcsRoot)
        {
            var data = new VcsRootEntry {VcsRoot = new VcsRoot {Id = vcsRoot.Id}};

            return m_caller.PostFormat<VcsRoot>(data, HttpContentTypes.ApplicationJson, HttpContentTypes.ApplicationJson,
                "/buildTypes/{0}/vcs-root-entries", locator);
        }

        public async Task<VcsRoot> AttachVcsRootAsync(BuildTypeLocator locator, VcsRoot vcsRoot)
        {
            var data = new VcsRootEntry { VcsRoot = new VcsRoot { Id = vcsRoot.Id } };

            return await m_caller.PostFormatAsync<VcsRoot>(data, HttpContentTypes.ApplicationJson, HttpContentTypes.ApplicationJson,
                "/buildTypes/{0}/vcs-root-entries", locator);
        }

        public void DetachVcsRoot(BuildTypeLocator locator, string vcsRootId)
        {
            m_caller.DeleteFormat("/buildTypes/{0}/vcs-root-entries/{1}", locator, vcsRootId);
        }

        public async Task DetachVcsRootAsync(BuildTypeLocator locator, string vcsRootId)
        {
            await m_caller.DeleteFormatAsync("/buildTypes/{0}/vcs-root-entries/{1}", locator, vcsRootId);
        }

        public void SetVcsRootValue(VcsRoot vcsRoot, VcsRootValue field, object value)
        {
            m_caller.PutFormat(value, HttpContentTypes.TextPlain, "/vcs-roots/id:{0}/{1}", vcsRoot.Id,
                ToCamelCase(field.ToString()));
        }

        public async Task SetVcsRootValueAsync(VcsRoot vcsRoot, VcsRootValue field, object value)
        {
            await m_caller.PutFormatAsync(value, HttpContentTypes.TextPlain, "/vcs-roots/id:{0}/{1}", vcsRoot.Id,
                ToCamelCase(field.ToString()));
        }

        public void SetConfigurationProperties(VcsRoot vcsRoot, string key, string value)
        {
            m_caller.PutFormat(value, HttpContentTypes.TextPlain, "/vcs-roots/id:{0}/properties/{1}", vcsRoot.Id, key);
        }

        public async Task SetConfigurationPropertiesAsync(VcsRoot vcsRoot, string key, string value)
        {
            await m_caller.PutFormatAsync(value, HttpContentTypes.TextPlain, "/vcs-roots/id:{0}/properties/{1}", vcsRoot.Id, key);
        }

        public void DeleteProperties(VcsRoot vcsRoot, string parameterName)
        {
            m_caller.DeleteFormat("/vcs-roots/id:{0}/properties/{1}", vcsRoot.Id, parameterName);
        }

        public async Task DeletePropertiesAsync(VcsRoot vcsRoot, string parameterName)
        {
            await m_caller.DeleteFormatAsync("/vcs-roots/id:{0}/properties/{1}", vcsRoot.Id, parameterName);
        }

        public VcsRoot CreateVcsRoot(VcsRoot vcsRoot, string projectId)
        {
            return m_caller.PostFormat<VcsRoot>(vcsRoot, HttpContentTypes.ApplicationJson,
                HttpContentTypes.ApplicationJson, "/vcs-roots",
                projectId);
        }

        public async Task<VcsRoot> CreateVcsRootAsync(VcsRoot vcsRoot, string projectId)
        {
            return await m_caller.PostFormatAsync<VcsRoot>(vcsRoot, HttpContentTypes.ApplicationJson,
                HttpContentTypes.ApplicationJson, "/vcs-roots",
                projectId);
        }

        public void DeleteVcsRoot(VcsRoot vcsRoot)
        {
            m_caller.DeleteFormat("/vcs-roots/id:{0}", vcsRoot.Id);
        }

        public async Task DeleteVcsRootAsync(VcsRoot vcsRoot)
        {
            await m_caller.DeleteFormatAsync("/vcs-roots/id:{0}", vcsRoot.Id);
        }

        private static string ToCamelCase(string s)
        {
            return char.ToLower(s.ToCharArray()[0]) + s.Substring(1);
        }
    }
}