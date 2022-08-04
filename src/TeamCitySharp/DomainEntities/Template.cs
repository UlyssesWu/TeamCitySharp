using Newtonsoft.Json;
using TeamCitySharp.ActionTypes;

namespace TeamCitySharp.DomainEntities
{
    public class Template
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("href")] public string Href { get; set; }

        [JsonProperty("projectId")] public string ProjectId { get; set; }

        [JsonProperty("projectName")] public string ProjectName { get; set; }

        public Template()
        {
            Id = "";
            Name = "";
            Href = "";
            ProjectId = "";
            ProjectName = "";
        }

        /// <summary>
        /// Used as Template locator (WithId)
        /// </summary>
        /// <param name="id"></param>
        public Template(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Used as Template locator in <see cref="IBuildConfigsAsync.AddBuildTemplateAsync"/>
        /// </summary>
        /// <param name="id"></param>
        public static Template WithId(string id) => new(id);

        public static Template WithName(string name) => new() { Name = name };

        public static implicit operator BuildConfig(Template config)
        {
            return new BuildConfig
            {
                Href = config.Href,
                Name = config.Name,
                ProjectId = config.ProjectId,
                ProjectName = config.ProjectName
            };
        }
    }
}