using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml;
using Newtonsoft.Json;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public class BuildConfigs : IBuildConfigs
    {
        private readonly ITeamCityCaller m_caller;
        private string m_fields;

        internal BuildConfigs(ITeamCityCaller caller)
        {
            m_caller = caller;
        }

        public BuildConfigs GetFields(string fields)
        {
            var newInstance = (BuildConfigs)MemberwiseClone();
            newInstance.m_fields = fields;
            return newInstance;
        }

        public List<BuildConfig> All()
        {
            var buildType =
                m_caller.Get<BuildTypeWrapper>(ActionHelper.CreateFieldUrl("/buildTypes", m_fields));

            return buildType.BuildType;
        }

        public async Task<List<BuildConfig>> AllAsync()
        {
            var buildType =
                await m_caller.GetAsync<BuildTypeWrapper>(ActionHelper.CreateFieldUrl("/buildTypes", m_fields));

            return buildType.BuildType;
        }

        public BuildConfig ByConfigurationName(string buildConfigName)
        {
            var build = m_caller.GetFormat<BuildConfig>(ActionHelper.CreateFieldUrl("/buildTypes/name:{0}", m_fields),
                buildConfigName);

            return build;
        }

        public async Task<BuildConfig> ByConfigurationNameAsync(string buildConfigName)
        {
            var build = await m_caller.GetFormatAsync<BuildConfig>(ActionHelper.CreateFieldUrl("/buildTypes/name:{0}", m_fields),
                buildConfigName);

            return build;
        }

        public BuildConfig ByConfigurationId(string buildConfigId)
        {
            var build = m_caller.GetFormat<BuildConfig>(ActionHelper.CreateFieldUrl("/buildTypes/id:{0}", m_fields),
                buildConfigId);

            return build;
        }

        public async Task<BuildConfig> ByConfigurationIdAsync(string buildConfigId)
        {
            var build = await m_caller.GetFormatAsync<BuildConfig>(ActionHelper.CreateFieldUrl("/buildTypes/id:{0}", m_fields),
                buildConfigId);

            return build;
        }

        public BuildConfig ByProjectNameAndConfigurationName(string projectName, string buildConfigName)
        {
            var build =
                m_caller.Get<BuildConfig>(
                    ActionHelper.CreateFieldUrl(
                        $"/projects/name:{projectName}/buildTypes/name:{buildConfigName}", m_fields));
            return build;
        }

        public async Task<BuildConfig> ByProjectNameAndConfigurationNameAsync(string projectName, string buildConfigName)
        {
            var build =
                await m_caller.GetAsync<BuildConfig>(
                    ActionHelper.CreateFieldUrl(
                        $"/projects/name:{projectName}/buildTypes/name:{buildConfigName}", m_fields));
            return build;
        }

        public BuildConfig ByProjectNameAndConfigurationId(string projectName, string buildConfigId)
        {
            var build =
                m_caller.Get<BuildConfig>(
                    ActionHelper.CreateFieldUrl(
                        $"/projects/name:{projectName}/buildTypes/id:{buildConfigId}", m_fields));
            return build;
        }

        public async Task<BuildConfig> ByProjectNameAndConfigurationIdAsync(string projectName, string buildConfigId)
        {
            var build =
                await m_caller.GetAsync<BuildConfig>(
                    ActionHelper.CreateFieldUrl(
                        $"/projects/name:{projectName}/buildTypes/id:{buildConfigId}", m_fields));
            return build;
        }

        public BuildConfig ByProjectIdAndConfigurationName(string projectId, string buildConfigName)
        {
            var build =
                m_caller.Get<BuildConfig>(
                    ActionHelper.CreateFieldUrl(
                        $"/projects/id:{projectId}/buildTypes/name:{Uri.EscapeDataString(buildConfigName)}", m_fields));
            return build;
        }

        public async Task<BuildConfig> ByProjectIdAndConfigurationNameAsync(string projectId, string buildConfigName)
        {
            var build =
                await m_caller.GetAsync<BuildConfig>(
                    ActionHelper.CreateFieldUrl(
                        $"/projects/id:{projectId}/buildTypes/name:{Uri.EscapeDataString(buildConfigName)}", m_fields));
            return build;
        }

        public BuildConfig ByProjectIdAndConfigurationId(string projectId, string buildConfigId)
        {
            var build =
                m_caller.Get<BuildConfig>(
                    ActionHelper.CreateFieldUrl(
                        $"/projects/id:{projectId}/buildTypes/id:{buildConfigId}", m_fields));
            return build;
        }

        public async Task<BuildConfig> ByProjectIdAndConfigurationIdAsync(string projectId, string buildConfigId)
        {
            var build =
                await m_caller.GetAsync<BuildConfig>(
                    ActionHelper.CreateFieldUrl(
                        $"/projects/id:{projectId}/buildTypes/id:{buildConfigId}", m_fields));
            return build;
        }

        public List<BuildConfig> ByProjectId(string projectId)
        {
            var buildWrapper =
                m_caller.GetFormat<BuildTypeWrapper>(
                    ActionHelper.CreateFieldUrl("/projects/id:{0}/buildTypes", m_fields), projectId);

            return buildWrapper?.BuildType ?? new List<BuildConfig>();
        }

        public async Task<List<BuildConfig>> ByProjectIdAsync(string projectId)
        {
            var buildWrapper =
                await m_caller.GetFormatAsync<BuildTypeWrapper>(
                    ActionHelper.CreateFieldUrl("/projects/id:{0}/buildTypes", m_fields), projectId);

            return buildWrapper?.BuildType ?? new List<BuildConfig>();
        }

        public List<BuildConfig> ByProjectName(string projectName)
        {
            var buildWrapper =
                m_caller.GetFormat<BuildTypeWrapper>(
                    ActionHelper.CreateFieldUrl("/projects/name:{0}/buildTypes", m_fields), projectName);

            return buildWrapper?.BuildType ?? new List<BuildConfig>();
        }

        public async Task<List<BuildConfig>> ByProjectNameAsync(string projectName)
        {
            var buildWrapper =
                await m_caller.GetFormatAsync<BuildTypeWrapper>(
                    ActionHelper.CreateFieldUrl("/projects/name:{0}/buildTypes", m_fields), projectName);

            return buildWrapper?.BuildType ?? new List<BuildConfig>();
        }

        public BuildConfig CreateConfiguration(BuildConfig buildConfig)
        {
            return m_caller.PostFormat<BuildConfig>(buildConfig, HttpContentTypes.ApplicationJson,
                HttpContentTypes.ApplicationJson, "/buildTypes");
        }

        public async Task<BuildConfig> CreateConfigurationAsync(BuildConfig buildConfig)
        {
            return await m_caller.PostFormatAsync<BuildConfig>(buildConfig, HttpContentTypes.ApplicationJson,
                HttpContentTypes.ApplicationJson, "/buildTypes");
        }

        public BuildConfig CreateConfiguration(string projectName, string configurationName)
        {
            return m_caller.PostFormat<BuildConfig>(configurationName, HttpContentTypes.TextPlain,
                HttpContentTypes.ApplicationJson, "/projects/name:{0}/buildTypes",
                projectName);
        }

        public BuildConfig CreateConfigurationByProjectId(string projectId, string configurationName)
        {
            return m_caller.PostFormat<BuildConfig>(configurationName, HttpContentTypes.TextPlain,
                HttpContentTypes.ApplicationJson, "/projects/id:{0}/buildTypes",
                projectId);
        }

        public async Task<BuildConfig> CreateConfigurationAsync(string projectName, string configurationName)
        {
            return await m_caller.PostFormatAsync<BuildConfig>(configurationName, HttpContentTypes.TextPlain,
                HttpContentTypes.ApplicationJson, "/projects/name:{0}/buildTypes",
                projectName);
        }

        internal HttpResponseMessage CopyBuildConfig(string buildConfigId, string buildConfigName, string destinationProjectId,
            string newBuildTypeId = "")
        {
            string xmlData;
            if (newBuildTypeId != "")
            {
                xmlData =
                    string.Format(
                        "<newBuildTypeDescription name='{0}' id='{2}' sourceBuildTypeLocator='id:{1}' copyAllAssociatedSettings='true' shareVCSRoots='false'/>",
                        buildConfigName, buildConfigId, newBuildTypeId);
            }
            else
            {
                xmlData =
                    $"<newBuildTypeDescription name='{buildConfigName}' sourceBuildTypeLocator='id:{buildConfigId}' copyAllAssociatedSettings='true' shareVCSRoots='false'/>";
            }

            var response = m_caller.Post(xmlData, HttpContentTypes.ApplicationXml,
                $"/projects/id:{destinationProjectId}/buildTypes",
                HttpContentTypes.ApplicationJson);
            return response;
        }

        internal async Task<HttpResponseMessage> CopyBuildConfigAsync(string buildConfigId, string buildConfigName,
            string destinationProjectId, string newBuildTypeId = "")
        {
            string xmlData;
            if (newBuildTypeId != "")
            {
                xmlData =
                    $"<newBuildTypeDescription name='{buildConfigName}' id='{newBuildTypeId}' sourceBuildTypeLocator='id:{buildConfigId}' copyAllAssociatedSettings='true' shareVCSRoots='false'/>";
            }
            else
            {
                xmlData =
                    $"<newBuildTypeDescription name='{buildConfigName}' sourceBuildTypeLocator='id:{buildConfigId}' copyAllAssociatedSettings='true' shareVCSRoots='false'/>";
            }

            var response = await m_caller.PostAsync(xmlData, HttpContentTypes.ApplicationXml,
                $"/projects/id:{destinationProjectId}/buildTypes",
                HttpContentTypes.ApplicationJson);
            return response;
        }

        public BuildConfig Copy(string buildConfigId, string buildConfigName, string destinationProjectId,
            string newBuildTypeId = "")
        {
            var response = CopyBuildConfig(buildConfigId, buildConfigName, destinationProjectId, newBuildTypeId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var buildConfig = JsonConvert.DeserializeObject<BuildConfig>(response.RawText());
                return buildConfig;
            }

            return new BuildConfig();
        }

        public async Task<BuildConfig> CopyAsync(string buildConfigId, string buildConfigName, string destinationProjectId,
            string newBuildTypeId = "")
        {
            var response = await CopyBuildConfigAsync(buildConfigId, buildConfigName, destinationProjectId,
                newBuildTypeId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var buildConfig = JsonConvert.DeserializeObject<BuildConfig>(await response.RawTextAsync());
                return buildConfig;
            }

            return new BuildConfig();
        }

        public Template CopyTemplate(string templateId, string templateName, string destinationProjectId,
            string newTemplateId = "")
        {
            var response = CopyTemplateQuery(templateId, templateName, destinationProjectId, newTemplateId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var template = JsonConvert.DeserializeObject<Template>(response.RawText());
                return template;
            }

            return new Template();
        }

        private HttpResponseMessage CopyTemplateQuery(string templateId, string templateName, string destinationProjectId,
            string newTemplateId)
        {
            var xmlData = newTemplateId != ""
                ? $"<newBuildTypeDescription name='{templateName}' id='{newTemplateId}' sourceBuildTypeLocator='id:{templateId}' copyAllAssociatedSettings='true' shareVCSRoots='false'/>"
                : $"<newBuildTypeDescription name='{templateName}' sourceBuildTypeLocator='id:{templateId}' copyAllAssociatedSettings='true' shareVCSRoots='false'/>";
            var response = m_caller.Post(xmlData, HttpContentTypes.ApplicationXml,
                $"/projects/id:{destinationProjectId}/templates",
                HttpContentTypes.ApplicationJson);
            return response;
        }

        public async Task<Template> CopyTemplateAsync(string templateId, string templateName, string destinationProjectId,
            string newTemplateId = "")
        {
            var response = await CopyTemplateQueryAsync(templateId, templateName, destinationProjectId, newTemplateId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var template = JsonConvert.DeserializeObject<Template>(await response.RawTextAsync());
                return template;
            }

            return new Template();
        }

        private async Task<HttpResponseMessage> CopyTemplateQueryAsync(string templateId, string templateName,
            string destinationProjectId, string newTemplateId)
        {
            var xmlData = newTemplateId != ""
                ? $"<newBuildTypeDescription name='{templateName}' id='{newTemplateId}' sourceBuildTypeLocator='id:{templateId}' copyAllAssociatedSettings='true' shareVCSRoots='false'/>"
                : $"<newBuildTypeDescription name='{templateName}' sourceBuildTypeLocator='id:{templateId}' copyAllAssociatedSettings='true' shareVCSRoots='false'/>";
            var response = await m_caller.PostAsync(xmlData, HttpContentTypes.ApplicationXml,
                $"/projects/id:{destinationProjectId}/templates",
                HttpContentTypes.ApplicationJson);
            return response;
        }

        public void SetConfigurationSetting(BuildTypeLocator locator, string settingName, string settingValue)
        {
            m_caller.PutFormat(settingValue, HttpContentTypes.TextPlain, "/buildTypes/{0}/settings/{1}", locator,
                settingName);
        }

        public async Task SetConfigurationSettingAsync(BuildTypeLocator locator, string settingName,
            string settingValue)
        {
            await m_caller.PutFormatAsync(settingValue, HttpContentTypes.TextPlain, "/buildTypes/{0}/settings/{1}",
                locator, settingName);
        }

        public bool GetConfigurationPauseStatus(BuildTypeLocator locator)
        {
            bool.TryParse(
                m_caller.GetRaw(ActionHelper.CreateFieldUrl($"/buildTypes/{locator}/paused/", m_fields)), out var result);
            return result;
        }

        public async Task<bool> GetConfigurationPauseStatusAsync(BuildTypeLocator locator)
        {
            var response = await m_caller.GetRawAsync(ActionHelper.CreateFieldUrl($"/buildTypes/{locator}/paused/",
                m_fields));
            bool.TryParse(response, out var result);
            return result;
        }

        public void SetConfigurationPauseStatus(BuildTypeLocator locator, bool isPaused)
        {
            m_caller.PutFormat(isPaused, HttpContentTypes.TextPlain, "/buildTypes/{0}/paused/", locator);
        }

        public async Task SetConfigurationPauseStatusAsync(BuildTypeLocator locator, bool isPaused)
        {
            await m_caller.PutFormatAsync(isPaused, HttpContentTypes.TextPlain, "/buildTypes/{0}/paused/", locator);
        }

        public void PostRawArtifactDependency(BuildTypeLocator locator, string rawXml)
        {
            m_caller.PostFormat<ArtifactDependency>(rawXml, HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson,
                "/buildTypes/{0}/artifact-dependencies", locator);
        }

        public async Task PostRawArtifactDependencyAsync(BuildTypeLocator locator, string rawXml)
        {
            await m_caller.PostFormatAsync<ArtifactDependency>(rawXml, HttpContentTypes.ApplicationXml,
                HttpContentTypes.ApplicationJson, "/buildTypes/{0}/artifact-dependencies", locator);
        }

        public void PostRawBuildStep(BuildTypeLocator locator, string rawXml)
        {
            m_caller.PostFormat<BuildConfig>(rawXml, HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson,
                "/buildTypes/{0}/steps", locator);
        }

        public async Task PostRawBuildStepAsync(BuildTypeLocator locator, string rawXml)
        {
            await m_caller.PostFormatAsync<BuildConfig>(rawXml, HttpContentTypes.ApplicationXml,
                HttpContentTypes.ApplicationJson, "/buildTypes/{0}/steps", locator);
        }

        public void PutRawBuildStep(BuildTypeLocator locator, string rawXml)
        {
            m_caller.PutFormat(rawXml, HttpContentTypes.ApplicationXml, "/buildTypes/{0}/steps", locator);
        }

        public async Task PutRawBuildStepAsync(BuildTypeLocator locator, string rawXml)
        {
            await m_caller.PutFormatAsync(rawXml, HttpContentTypes.ApplicationXml, "/buildTypes/{0}/steps", locator);
        }

        public BuildSteps GetRawBuildStep(BuildTypeLocator locator)
        {
            return m_caller.GetFormat<BuildSteps>("/buildTypes/{0}/steps", locator);
        }

        public async Task<BuildSteps> GetRawBuildStepAsync(BuildTypeLocator locator)
        {
            return await m_caller.GetFormatAsync<BuildSteps>("/buildTypes/{0}/steps", locator);
        }

        public void PostRawBuildTrigger(BuildTypeLocator locator, string rawXml)
        {
            m_caller.PostFormat(rawXml, HttpContentTypes.ApplicationXml, "/buildTypes/{0}/triggers", locator);
        }

        public async Task PostRawBuildTriggerAsync(BuildTypeLocator locator, string rawXml)
        {
            await m_caller.PostFormatAsync(rawXml, HttpContentTypes.ApplicationXml, "/buildTypes/{0}/triggers", locator);
        }

        public void SetArtifactDependency(BuildTypeLocator locator, ArtifactDependency dependency)
        {
            m_caller.PostFormat<ArtifactDependency>(dependency, HttpContentTypes.ApplicationJson,
                HttpContentTypes.ApplicationJson,
                "/buildTypes/{0}/artifact-dependencies", locator);
        }

        public async Task SetArtifactDependencyAsync(BuildTypeLocator locator, ArtifactDependency dependency)
        {
            await m_caller.PostFormatAsync<ArtifactDependency>(dependency, HttpContentTypes.ApplicationJson,
                HttpContentTypes.ApplicationJson,
                "/buildTypes/{0}/artifact-dependencies", locator);
        }

        public void SetSnapshotDependency(BuildTypeLocator locator, SnapshotDependency dependency)
        {
            m_caller.PostFormat<SnapshotDependency>(dependency, HttpContentTypes.ApplicationJson,
                HttpContentTypes.ApplicationJson,
                "/buildTypes/{0}/snapshot-dependencies", locator);
        }

        public async Task SetSnapshotDependencyAsync(BuildTypeLocator locator, SnapshotDependency dependency)
        {
            await m_caller.PostFormatAsync<SnapshotDependency>(dependency, HttpContentTypes.ApplicationJson,
                HttpContentTypes.ApplicationJson,
                "/buildTypes/{0}/snapshot-dependencies", locator);
        }

        public void SetTrigger(BuildTypeLocator locator, BuildTrigger trigger)
        {
            m_caller.PostFormat<BuildTrigger>(trigger, HttpContentTypes.ApplicationJson, HttpContentTypes.ApplicationJson,
                "/buildTypes/{0}/triggers", locator);
        }

        public async Task SetTriggerAsync(BuildTypeLocator locator, BuildTrigger trigger)
        {
            await m_caller.PostFormatAsync<BuildTrigger>(trigger, HttpContentTypes.ApplicationJson,
                HttpContentTypes.ApplicationJson,
                "/buildTypes/{0}/triggers", locator);
        }

        public void SetConfigurationParameter(BuildTypeLocator locator, string key, string value)
        {
            m_caller.PutFormat(value, HttpContentTypes.TextPlain, "/buildTypes/{0}/parameters/{1}", locator, key);
        }

        public async Task SetConfigurationParameterAsync(BuildTypeLocator locator, string key, string value)
        {
            await m_caller.PutFormatAsync(value, HttpContentTypes.TextPlain, "/buildTypes/{0}/parameters/{1}", locator,
                key);
        }

        public void DeleteConfiguration(BuildTypeLocator locator)
        {
            m_caller.DeleteFormat("/buildTypes/{0}", locator);
        }

        public async Task DeleteConfigurationAsync(BuildTypeLocator locator)
        {
            await m_caller.DeleteFormatAsync("/buildTypes/{0}", locator);
        }
        
        public void DeleteAllBuildTypeParameters(BuildTypeLocator locator)
        {
            m_caller.DeleteFormat("/buildTypes/{0}/parameters", locator);
        }

        public async Task DeleteAllBuildTypeParametersAsync(BuildTypeLocator locator)
        {
            await m_caller.DeleteFormatAsync("/buildTypes/{0}/parameters", locator);
        }

        public void PutAllBuildTypeParameters(BuildTypeLocator locator, IDictionary<string, string> parameters)
        {
            if (locator == null) throw new ArgumentNullException(nameof(locator));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var sw = new StringWriter();
            using (var writer = new XmlTextWriter(sw))
            {
                writer.WriteStartElement("properties");
                foreach (var parameter in parameters)
                {
                    writer.WriteStartElement("property");
                    writer.WriteAttributeString("name", parameter.Key);
                    writer.WriteAttributeString("value", parameter.Value);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            m_caller.PutFormat(sw.ToString(), HttpContentTypes.ApplicationXml, "/buildTypes/{0}/parameters", locator);
        }

        public async Task PutAllBuildTypeParametersAsync(BuildTypeLocator locator, IDictionary<string, string> parameters)
        {
            if (locator == null) throw new ArgumentNullException(nameof(locator));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var sw = new StringWriter();
            using (var writer = new XmlTextWriter(sw))
            {
                writer.WriteStartElement("properties");
                foreach (var parameter in parameters)
                {
                    writer.WriteStartElement("property");
                    writer.WriteAttributeString("name", parameter.Key);
                    writer.WriteAttributeString("value", parameter.Value);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            await m_caller.PutFormatAsync(sw.ToString(), HttpContentTypes.ApplicationXml, "/buildTypes/{0}/parameters",
                locator);
        }

        public void DownloadConfiguration(BuildTypeLocator locator, Action<string> downloadHandler)
        {
            var url = $"/buildTypes/{locator}";
            m_caller.GetDownloadFormat(downloadHandler, url);
        }

        public async Task DownloadConfigurationAsync(BuildTypeLocator locator, Action<string> downloadHandler)
        {
            var url = $"/buildTypes/{locator}";
            await m_caller.GetDownloadFormatAsync(downloadHandler, url);
        }

        public void PostRawAgentRequirement(BuildTypeLocator locator, string rawXml)
        {
            m_caller.PostFormat(rawXml, HttpContentTypes.ApplicationXml, "/buildTypes/{0}/agent-requirements", locator);
        }

        public async Task PostRawAgentRequirementAsync(BuildTypeLocator locator, string rawXml)
        {
            await m_caller.PostFormatAsync(rawXml, HttpContentTypes.ApplicationXml, "/buildTypes/{0}/agent-requirements",
                locator);
        }

        public void DeleteBuildStep(BuildTypeLocator locator, string buildStepId)
        {
            m_caller.DeleteFormat("/buildTypes/{0}/steps/{1}", locator, buildStepId);
        }

        public async Task DeleteBuildStepAsync(BuildTypeLocator locator, string buildStepId)
        {
            await m_caller.DeleteFormatAsync("/buildTypes/{0}/steps/{1}", locator, buildStepId);
        }

        public void DeleteArtifactDependency(BuildTypeLocator locator, string artifactDependencyId)
        {
            m_caller.DeleteFormat("/buildTypes/{0}/artifact-dependencies/{1}", locator, artifactDependencyId);
        }

        public async Task DeleteArtifactDependencyAsync(BuildTypeLocator locator, string artifactDependencyId)
        {
            await m_caller.DeleteFormatAsync("/buildTypes/{0}/artifact-dependencies/{1}", locator, artifactDependencyId);
        }

        public void DeleteAgentRequirement(BuildTypeLocator locator, string agentRequirementId)
        {
            m_caller.DeleteFormat("/buildTypes/{0}/agent-requirements/{1}", locator, agentRequirementId);
        }

        public async Task DeleteAgentRequirementAsync(BuildTypeLocator locator, string agentRequirementId)
        {
            await m_caller.DeleteFormatAsync("/buildTypes/{0}/agent-requirements/{1}", locator, agentRequirementId);
        }

        public void DeleteParameter(BuildTypeLocator locator, string parameterName)
        {
            m_caller.DeleteFormat("/buildTypes/{0}/parameters/{1}", locator, parameterName);
        }

        public async Task DeleteParameterAsync(BuildTypeLocator locator, string parameterName)
        {
            await m_caller.DeleteFormatAsync("/buildTypes/{0}/parameters/{1}", locator, parameterName);
        }

        public void DeleteBuildTrigger(BuildTypeLocator locator, string buildTriggerId)
        {
            m_caller.DeleteFormat("/buildTypes/{0}/triggers/{1}", locator, buildTriggerId);
        }

        public async Task DeleteBuildTriggerAsync(BuildTypeLocator locator, string buildTriggerId)
        {
            await m_caller.DeleteFormatAsync("/buildTypes/{0}/triggers/{1}", locator, buildTriggerId);
        }

        public void SetBuildTypeTemplate(BuildTypeLocator locatorBuildType, BuildTypeLocator locatorTemplate)
        {
            m_caller.PutFormat(locatorTemplate.ToString(), HttpContentTypes.TextPlain, "/buildTypes/{0}/template",
                locatorBuildType);
        }

        public async Task SetBuildTypeTemplateAsync(BuildTypeLocator locatorBuildType, BuildTypeLocator locatorTemplate)
        {
            await m_caller.PutFormatAsync(locatorTemplate.ToString(), HttpContentTypes.TextPlain,
                "/buildTypes/{0}/template", locatorBuildType);
        }

        public void DeleteSnapshotDependency(BuildTypeLocator locator, string snapshotDependencyId)
        {
            m_caller.DeleteFormat("/buildTypes/{0}/snapshot-dependencies/{1}", locator, snapshotDependencyId);
        }

        public async Task DeleteSnapshotDependencyAsync(BuildTypeLocator locator, string snapshotDependencyId)
        {
            await m_caller.DeleteFormatAsync("/buildTypes/{0}/snapshot-dependencies/{1}", locator, snapshotDependencyId);
        }

        public void PostRawSnapshotDependency(BuildTypeLocator locator, XmlElement rawXml)
        {
            m_caller.PostFormat(rawXml.OuterXml, HttpContentTypes.ApplicationXml,
                "/buildTypes/{0}/snapshot-dependencies", locator);
        }

        public async Task PostRawSnapshotDependencyAsync(BuildTypeLocator locator, XmlElement rawXml)
        {
            await m_caller.PostFormatAsync(rawXml.OuterXml, HttpContentTypes.ApplicationXml,
                "/buildTypes/{0}/snapshot-dependencies", locator);
        }

        public BuildConfig BuildType(BuildTypeLocator locator)
        {
            var build = m_caller.GetFormat<BuildConfig>(ActionHelper.CreateFieldUrl("/buildTypes/{0}", m_fields),
                locator);

            return build;
        }

        public async Task<BuildConfig> BuildTypeAsync(BuildTypeLocator locator)
        {
            var build = await m_caller.GetFormatAsync<BuildConfig>(ActionHelper.CreateFieldUrl("/buildTypes/{0}", m_fields),
                locator);

            return build;
        }

        public void SetBuildTypeVariable(BuildTypeLocator locatorBuildType, string nameVariable, string value)
        {
            m_caller.PutFormat(value, HttpContentTypes.TextPlain, "/buildTypes/{0}/{1}", locatorBuildType,
                nameVariable);
        }

        public async Task SetBuildTypeVariableAsync(BuildTypeLocator locatorBuildType, string nameVariable, string value)
        {
            await m_caller.PutFormatAsync(value, HttpContentTypes.TextPlain, "/buildTypes/{0}/{1}", locatorBuildType,
                nameVariable);
        }

        public bool ModifyTrigger(string buildTypeId, string triggerId, string newBt)
        {
            //Get data from the old trigger
            var urlExtractAllTriggersOld = $"/buildTypes/id:{buildTypeId}/triggers";
            var triggers = m_caller.GetFormat<BuildTriggers>(urlExtractAllTriggersOld);
            foreach (var trigger in triggers.Trigger.OrderByDescending(m => m.Id))
            {
                if (trigger.Type != "buildDependencyTrigger") continue;

                foreach (var property in trigger.Properties.Property)
                {
                    if (property.Name != "dependsOn") continue;

                    if (triggerId != property.Value) continue;

                    property.Value = newBt;

                    var urlNewTrigger = $"/buildTypes/id:{buildTypeId}/triggers";
                    var response = m_caller.Post(trigger, HttpContentTypes.ApplicationJson, urlNewTrigger,
                        HttpContentTypes.ApplicationJson);
                    if (response.StatusCode != HttpStatusCode.OK) continue;

                    var urlDeleteOld = $"/buildTypes/id:{buildTypeId}/triggers/{trigger.Id}";
                    m_caller.Delete(urlDeleteOld);
                    if (response.StatusCode == HttpStatusCode.OK)
                        return true;
                }
            }

            return false;
        }

        public async Task<bool> ModifyTriggerAsync(string buildTypeId, string triggerId, string newBt)
        {
            //Get data from the old trigger
            var urlExtractAllTriggersOld = $"/buildTypes/id:{buildTypeId}/triggers";
            var triggers = await m_caller.GetFormatAsync<BuildTriggers>(urlExtractAllTriggersOld);
            foreach (var trigger in triggers.Trigger.OrderByDescending(m => m.Id))
            {
                if (trigger.Type != "buildDependencyTrigger") continue;

                foreach (var property in trigger.Properties.Property)
                {
                    if (property.Name != "dependsOn") continue;

                    if (triggerId != property.Value) continue;

                    property.Value = newBt;

                    var urlNewTrigger = $"/buildTypes/id:{buildTypeId}/triggers";
                    var response = await m_caller.PostAsync(trigger, HttpContentTypes.ApplicationJson, urlNewTrigger,
                        HttpContentTypes.ApplicationJson);
                    if (response.StatusCode != HttpStatusCode.OK) continue;

                    var urlDeleteOld = $"/buildTypes/id:{buildTypeId}/triggers/{trigger.Id}";
                    await m_caller.DeleteAsync(urlDeleteOld);
                    if (response.StatusCode == HttpStatusCode.OK)
                        return true;
                }
            }

            return false;
        }

        public bool ModifySnapshotDependencies(string buildTypeId, string oldDependencyConfigurationId, string newBt)
        {
            var urlExtractOld = $"/buildTypes/id:{buildTypeId}/snapshot-dependencies/{oldDependencyConfigurationId}";
            var snapshot = (CustomSnapshotDependency)m_caller.GetFormat<SnapshotDependency>(urlExtractOld);
            snapshot.Id = newBt;
            snapshot.SourceBuildType.Id = newBt;

            var urlNewTrigger = $"/buildTypes/id:{buildTypeId}/snapshot-dependencies";

            var response = m_caller.Post(snapshot, HttpContentTypes.ApplicationJson, urlNewTrigger,
                HttpContentTypes.ApplicationJson);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var urlDeleteOld = $"/buildTypes/id:{buildTypeId}/snapshot-dependencies/{oldDependencyConfigurationId}";
                m_caller.Delete(urlDeleteOld);
                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
            }

            return false;
        }

        public async Task<bool> ModifySnapshotDependenciesAsync(string buildTypeId, string oldDependencyConfigurationId,
            string newBt)
        {
            var urlExtractOld = $"/buildTypes/id:{buildTypeId}/snapshot-dependencies/{oldDependencyConfigurationId}";
            var snapshot = (CustomSnapshotDependency)await m_caller.GetFormatAsync<SnapshotDependency>(urlExtractOld);
            snapshot.Id = newBt;
            snapshot.SourceBuildType.Id = newBt;

            var urlNewTrigger = $"/buildTypes/id:{buildTypeId}/snapshot-dependencies";

            var response = await m_caller.PostAsync(snapshot, HttpContentTypes.ApplicationJson, urlNewTrigger,
                HttpContentTypes.ApplicationJson);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var urlDeleteOld = $"/buildTypes/id:{buildTypeId}/snapshot-dependencies/{oldDependencyConfigurationId}";
                await m_caller.DeleteAsync(urlDeleteOld);
                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
            }

            return false;
        }

        public bool ModifyArtifactDependencies(string buildTypeId, string oldDependencyConfigurationId, string newBt)
        {
            var urlAllExtractOld = $"/buildTypes/id:{buildTypeId}/artifact-dependencies";
            var artifacts = (CustomArtifactDependencies)m_caller.GetFormat<ArtifactDependencies>(urlAllExtractOld);

            foreach (var artifact in artifacts.ArtifactDependency.OrderByDescending(m => m.Id))
            {
                if (oldDependencyConfigurationId != artifact.SourceBuildType.Id) continue;

                var oldArtifactId = artifact.Id;
                artifact.SourceBuildType.Id = newBt;
                artifact.Id = null;

                var urlNewTrigger = $"/buildTypes/id:{buildTypeId}/artifact-dependencies";
                
                var response = m_caller.Post(artifact, HttpContentTypes.ApplicationJson, urlNewTrigger,
                    HttpContentTypes.ApplicationJson);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var urlDeleteOld = $"/buildTypes/id:{buildTypeId}/artifact-dependencies/{oldArtifactId}";
                    m_caller.Delete(urlDeleteOld);
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }

            return false;
        }

        public async Task<bool> ModifyArtifactDependenciesAsync(string buildTypeId, string oldDependencyConfigurationId,
            string newBt)
        {
            var urlAllExtractOld = $"/buildTypes/id:{buildTypeId}/artifact-dependencies";
            var artifacts = (CustomArtifactDependencies)await m_caller.GetFormatAsync<ArtifactDependencies>(urlAllExtractOld);

            foreach (var artifact in artifacts.ArtifactDependency.OrderByDescending(m => m.Id))
            {
                if (oldDependencyConfigurationId != artifact.SourceBuildType.Id) continue;

                var oldArtifactId = artifact.Id;
                artifact.SourceBuildType.Id = newBt;
                artifact.Id = null;

                var urlNewTrigger = $"/buildTypes/id:{buildTypeId}/artifact-dependencies";

                var response = await m_caller.PostAsync(artifact, HttpContentTypes.ApplicationJson, urlNewTrigger,
                    HttpContentTypes.ApplicationJson);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var urlDeleteOld = $"/buildTypes/id:{buildTypeId}/artifact-dependencies/{oldArtifactId}";
                    await m_caller.DeleteAsync(urlDeleteOld);
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }

            return false;
        }

        public Branches GetBranchesByBuildConfigurationId(string buildTypeId, BranchLocator locator = null)
        {
            var locatorString = (locator != null) ? $"?locator={locator}" : "";
            var branches =
                m_caller.Get<Branches>(
                    ActionHelper.CreateFieldUrl(
                        $"/buildTypes/id:{buildTypeId}/branches{locatorString}", m_fields));
            return branches;
        }

        public async Task<Branches> GetBranchesByBuildConfigurationIdAsync(string buildTypeId,
            BranchLocator locator = null)
        {
            var locatorString = (locator != null) ? $"?locator={locator}" : "";
            var branches =
                await m_caller.GetAsync<Branches>(
                    ActionHelper.CreateFieldUrl(
                        $"/buildTypes/id:{buildTypeId}/branches{locatorString}", m_fields));
            return branches;
        }

        public ArtifactDependencies GetArtifactDependencies(string buildTypeId)
        {
            var artifactDependencies =
                m_caller.Get<ArtifactDependencies>(
                    ActionHelper.CreateFieldUrl(
                        $"/buildTypes/id:{buildTypeId}/artifact-dependencies", m_fields));
            return artifactDependencies;
        }

        public async Task<ArtifactDependencies> GetArtifactDependenciesAsync(string buildTypeId)
        {
            var artifactDependencies =
                await m_caller.GetAsync<ArtifactDependencies>(
                    ActionHelper.CreateFieldUrl(
                        $"/buildTypes/id:{buildTypeId}/artifact-dependencies", m_fields));
            return artifactDependencies;
        }

        public SnapshotDependencies GetSnapshotDependencies(string buildTypeId)
        {
            var snapshotDependencies =
                m_caller.Get<SnapshotDependencies>(
                    ActionHelper.CreateFieldUrl(
                        $"/buildTypes/id:{buildTypeId}/snapshot-dependencies", m_fields));
            return snapshotDependencies;
        }

        public async Task<SnapshotDependencies> GetSnapshotDependenciesAsync(string buildTypeId)
        {
            var snapshotDependencies =
                await m_caller.GetAsync<SnapshotDependencies>(
                    ActionHelper.CreateFieldUrl(
                        $"/buildTypes/id:{buildTypeId}/snapshot-dependencies", m_fields));
            return snapshotDependencies;
        }

        public Template GetTemplate(BuildTypeLocator locator)
        {
            try
            {
                var templateWrapper =
                    m_caller.GetFormat<Template>(ActionHelper.CreateFieldUrl("/buildTypes/{0}/template", m_fields), locator);
                return templateWrapper;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Template> GetTemplateAsync(BuildTypeLocator locator)
        {
            try
            {
                var templateWrapper =
                    await m_caller.GetFormatAsync<Template>(ActionHelper.CreateFieldUrl("/buildTypes/{0}/template", m_fields),
                        locator);
                return templateWrapper;
            }
            catch
            {
                return null;
            }
        }

        public Templates GetTemplates(BuildTypeLocator locator)
        {
            try
            {
                var templateWrapper =
                    m_caller.GetFormat<Templates>(ActionHelper.CreateFieldUrl("/buildTypes/{0}/templates", m_fields), locator);
                return templateWrapper;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Templates> GetTemplatesAsync(BuildTypeLocator locator)
        {
            try
            {
                var templateWrapper =
                    await m_caller.GetFormatAsync<Templates>(ActionHelper.CreateFieldUrl("/buildTypes/{0}/templates", m_fields),
                        locator);
                return templateWrapper;
            }
            catch
            {
                return null;
            }
        }

        public void AttachTemplate(BuildTypeLocator locator, string templateId)
        {
            m_caller.PutFormat(templateId, HttpContentTypes.TextPlain, "/buildTypes/{0}/template", locator);
        }

        public async Task AttachTemplateAsync(BuildTypeLocator locator, string templateId)
        {
            await m_caller.PutFormatAsync(templateId, HttpContentTypes.TextPlain, "/buildTypes/{0}/template", locator);
        }

        public void AttachTemplates(BuildTypeLocator locator, Templates templateList)
        {
            m_caller.PutFormat<Templates>(templateList, HttpContentTypes.ApplicationJson, HttpContentTypes.ApplicationJson,
                "/buildTypes/{0}/templates", locator);
        }

        public async Task AttachTemplatesAsync(BuildTypeLocator locator, Templates templateList)
        {
            await m_caller.PutFormatAsync<Templates>(templateList, HttpContentTypes.ApplicationJson,
                HttpContentTypes.ApplicationJson, "/buildTypes/{0}/templates", locator);
        }

        public void DetachTemplate(BuildTypeLocator locator)
        {
            m_caller.DeleteFormat("/buildTypes/{0}/template", locator);
        }

        public async Task DetachTemplateAsync(BuildTypeLocator locator)
        {
            await m_caller.DeleteFormatAsync("/buildTypes/{0}/template", locator);
        }

        public void DetachTemplates(BuildTypeLocator locator)
        {
            m_caller.DeleteFormat("/buildTypes/{0}/templates", locator);
        }

        public async Task DetachTemplatesAsync(BuildTypeLocator locator)
        {
            await m_caller.DeleteFormatAsync("/buildTypes/{0}/templates", locator);
        }

        #region Custom structure for copy

        internal class CustomSourceBuildType
        {
            [JsonProperty("id")] internal string Id { get; set; }
        }

        #region Artifact

        internal class CustomArtifactDependencies
        {
            [JsonProperty("artifact-dependency")] public List<CustomArtifactDependency> ArtifactDependency { get; set; }

            public static explicit operator CustomArtifactDependencies(ArtifactDependencies artifactDependencies)
            {
                var tmpArtifactDependencies = new CustomArtifactDependencies
                    { ArtifactDependency = new List<CustomArtifactDependency>() };
                foreach (var currentArtifactDependency in artifactDependencies.ArtifactDependency)
                {
                    tmpArtifactDependencies.ArtifactDependency.Add(new CustomArtifactDependency
                    {
                        Id = currentArtifactDependency.Id,
                        Type = currentArtifactDependency.Type,
                        Properties = currentArtifactDependency.Properties,
                        SourceBuildType = new CustomSourceBuildType { Id = currentArtifactDependency.SourceBuildType.Id }
                    });
                }

                return tmpArtifactDependencies;
            }
        }

        internal class CustomArtifactDependency
        {
            [JsonProperty("source-buildType")] internal CustomSourceBuildType SourceBuildType { get; set; }

            [JsonProperty("id")] internal string Id { get; set; }

            [JsonProperty("type")] internal string Type { get; set; }

            [JsonProperty("properties")] internal Properties Properties { get; set; }

            public static explicit operator CustomArtifactDependency(SnapshotDependency v)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Snapshot

        internal class CustomSnapshotDependencies
        {
            [JsonProperty("snapshot-dependency")] public List<CustomSnapshotDependency> SnapshotDependency { get; set; }

            public static explicit operator CustomSnapshotDependencies(SnapshotDependencies snapshotDependencies)
            {
                var tmpSnapshotDependencies = new CustomSnapshotDependencies
                    { SnapshotDependency = new List<CustomSnapshotDependency>() };
                foreach (var currentSnapshotDependency in snapshotDependencies.SnapshotDependency)
                {
                    tmpSnapshotDependencies.SnapshotDependency.Add(new CustomSnapshotDependency
                    {
                        Id = currentSnapshotDependency.Id,
                        Type = currentSnapshotDependency.Type,
                        Properties = currentSnapshotDependency.Properties,
                        SourceBuildType = new CustomSourceBuildType { Id = currentSnapshotDependency.SourceBuildType.Id }
                    });
                }

                return tmpSnapshotDependencies;
            }
        }

        internal class CustomSnapshotDependency
        {
            [JsonProperty("source-buildType")] internal CustomSourceBuildType SourceBuildType { get; set; }

            [JsonProperty("id")] internal string Id { get; set; }

            [JsonProperty("type")] internal string Type { get; set; }

            [JsonProperty("properties")] internal Properties Properties { get; set; }

            public static explicit operator CustomSnapshotDependency(SnapshotDependency snapshotDependency)
            {
                return new CustomSnapshotDependency
                {
                    Id = snapshotDependency.Id,
                    Type = snapshotDependency.Type,
                    Properties = snapshotDependency.Properties,
                    SourceBuildType = new CustomSourceBuildType { Id = snapshotDependency.SourceBuildType.Id }
                };
            }
        }

        #endregion

        #endregion
    }
}