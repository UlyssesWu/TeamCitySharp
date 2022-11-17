using System;
using Newtonsoft.Json;

namespace TeamCitySharp.DomainEntities
{
    public class RelatedIssues
    {
        [JsonProperty("href")] public string Href { get; set; }
    }

    public class ArtifactFiles
    {
        [JsonProperty("file")] public ArtifactFile[] File { get; set; }
        [JsonProperty("count")] public int Count { get; set; }
    }

    public class ArtifactFile
    {
        [JsonProperty("href")] public string Href { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("size")] public uint Size { get; set; }
        [JsonProperty("modificationTime")]
        [JsonConverter(typeof(TeamCityDateTimeConverter))]
        public DateTime ModificationTime { get; set; }
        
        
    }

    public class ArtifactContent
    {
        [JsonProperty("href")] public string Href { get; set; }
    }
}