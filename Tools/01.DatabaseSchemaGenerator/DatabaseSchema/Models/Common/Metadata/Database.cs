using Newtonsoft.Json;

namespace CodeGenerator.Models.Common
{

    public class Database
    {
        [JsonProperty(PropertyName = "dialect")]
        public string Dialect { get; set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }

}