using Newtonsoft.Json;
using System.Collections.Generic;

namespace Client.Modules.Utils.DAL.Common
{

    public class Metadata
    {
        [JsonProperty(PropertyName = "database")]
        public Database Database { get; set; }

        [JsonProperty(PropertyName = "namespace")]
        public string Namespace { get; set; }

        [JsonProperty(PropertyName = "max")]
        public int Max { get; set; }

        [JsonProperty(PropertyName = "multiplicity")]
        public Multiplicity Multiplicity { get; set; }

        [JsonProperty(PropertyName = "entityTypes")]
        public Dictionary<string, EntityType> EntityTypes { get; set; }

        [JsonProperty(PropertyName = "functions")]
        public Operation[] Functions { get; set; }

        [JsonProperty(PropertyName = "actions")]
        public Operation[] Actions { get; set; }
    }

}