using Newtonsoft.Json;

namespace TeamCitySharp.DomainEntities
{
    public class BuildCancelRequest
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("readdIntoQueue")]
        public bool ReAddIntoQueue { get; set; }
    }
}
