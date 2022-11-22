using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace TeamCitySharp.DomainEntities
{
    public class RelatedIssues
    {
        [JsonProperty("href")] public string Href { get; set; }
    }

    public class ArtifactFiles
    {
        [JsonProperty("file")] public ArtifactItem[] File { get; set; }
        [JsonProperty("count")] public int Count { get; set; }
    }

    /// <summary>
    /// Can be a file or a folder
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class ArtifactItem
    {
        /// <summary>
        /// Note: This url leads to a metadata json. use <see cref="ArtifactContent.Href"/> for download.
        /// </summary>
        [JsonProperty("href")] public string Href { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("size")] public uint Size { get; set; }
        [JsonProperty("modificationTime")]
        public DateTime ModificationTime { get; set; }
        [JsonProperty("content")] public ArtifactContent Content { get; set; }
        [JsonProperty("children")] public ArtifactItem Children { get; set; }
    }

    public class ArtifactContent
    {
        [JsonProperty("href")] public string Href { get; set; }
    }
}