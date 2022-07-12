using System.Collections.Generic;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public interface IVcsRoots : IVcsRootsAsync
    {
        VcsRoots GetFields(string fields);
        List<VcsRoot> All();
        VcsRoot ById(string vcsRootId);
        VcsRoot AttachVcsRoot(BuildTypeLocator locator, VcsRoot vcsRoot);
        void DetachVcsRoot(BuildTypeLocator locator, string vcsRootId);
        void SetVcsRootValue(VcsRoot vcsRoot, VcsRootValue field, object value);
        VcsRoot CreateVcsRoot(VcsRoot configurationName, string projectId);
        void SetConfigurationProperties(VcsRoot vcsRootId, string key, string value);
        void DeleteProperties(VcsRoot vcsRootId, string parameterName);
        void DeleteVcsRoot(VcsRoot vcsRoot);
    }

    public interface IVcsRootsAsync
    {
        Task<List<VcsRoot>> AllAsync();
        Task<VcsRoot> ByIdAsync(string vcsRootId);
        Task<VcsRoot> AttachVcsRootAsync(BuildTypeLocator locator, VcsRoot vcsRoot);
        Task DetachVcsRootAsync(BuildTypeLocator locator, string vcsRootId);
        Task SetVcsRootValueAsync(VcsRoot vcsRoot, VcsRootValue field, object value);
        Task<VcsRoot> CreateVcsRootAsync(VcsRoot configurationName, string projectId);
        Task SetConfigurationPropertiesAsync(VcsRoot vcsRootId, string key, string value);
        Task DeletePropertiesAsync(VcsRoot vcsRootId, string parameterName);
        Task DeleteVcsRootAsync(VcsRoot vcsRoot);
    }
}