using Newtonsoft.Json;

namespace Client.Models.Utils.DAL.Common
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