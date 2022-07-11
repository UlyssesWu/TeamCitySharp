using System.Collections.Generic;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public interface IProjects : IProjectsAsync
    {
        List<Project> All();
        Projects GetFields(string fields);
        Project ByName(string projectLocatorName);
        Project ById(string projectLocatorId);
        Project Details(Project project);
        Project Create(string projectName);
        Project Create(string projectName, string sourceId, string projectId = "");
        Project Move(string projectId, string destinationId);
        Project Copy(string sourceProjectId, string newProjectName, string newProjectId, string parentProjectId = "");
        string GenerateID(string projectName);
        void Delete(string projectName);
        void DeleteById(string projectId);
        void DeleteProjectParameter(string projectName, string parameterName);
        void SetProjectParameter(string projectName, string settingName, string settingValue);
        bool ModifyParameters(string projectId, string mainProjectBranch, string variablePath);
        bool ModifySettings(string projectId, string description, string fullProjectName);
        Branches GetBranchesByBuildProjectId(string projectId, BranchLocator locator = null);
        ProjectFeatures GetProjectFeatures(string projectLocatorId);
        ProjectFeature GetProjectFeatureByProjectFeature(string projectLocatorId, string projectFeatureId);
        ProjectFeature CreateProjectFeature(string projectId, ProjectFeature projectFeature);
        void DeleteProjectFeature(string projectId, string projectFeatureId);
    }

    public interface IProjectsAsync
    {
        Task<List<Project>> AllAsync();
        Task<Projects> GetFieldsAsync(string fields);
        Task<Project> ByNameAsync(string projectLocatorName);
        Task<Project> ByIdAsync(string projectLocatorId);
        Task<Project> DetailsAsync(Project project);
        Task<Project> CreateAsync(string projectName);
        Task<Project> CreateAsync(string projectName, string sourceId, string projectId = "");
        Task<Project> MoveAsync(string projectId, string destinationId);
        Task<Project> CopyAsync(string sourceProjectId, string newProjectName, string newProjectId, string parentProjectId = "");
        Task<string> GenerateIDAsync(string projectName);
        Task DeleteAsync(string projectName);
        Task DeleteByIdAsync(string projectId);
        Task DeleteProjectParameterAsync(string projectName, string parameterName);
        Task SetProjectParameterAsync(string projectName, string settingName, string settingValue);
        Task<bool> ModifyParametersAsync(string projectId, string mainProjectBranch, string variablePath);
        Task<bool> ModifySettingsAsync(string projectId, string description, string fullProjectName);
        Task<Branches> GetBranchesByBuildProjectIdAsync(string projectId, BranchLocator locator = null);
        Task<ProjectFeatures> GetProjectFeaturesAsync(string projectLocatorId);
        Task<ProjectFeature> GetProjectFeatureByProjectFeatureAsync(string projectLocatorId, string projectFeatureId);
        Task<ProjectFeature> CreateProjectFeatureAsync(string projectId, ProjectFeature projectFeature);
        Task DeleteProjectFeatureAsync(string projectId, string projectFeatureId);
    }
}