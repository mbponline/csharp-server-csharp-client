using Newtonsoft.Json;
using System.Collections.Generic;

namespace NavyBlueDtos.MetadataSrv
{

    public class Metadata
    {
        [JsonProperty(PropertyName = "dialect")]
        public string Dialect { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "namespace")]
        public string Namespace { get; set; }

        [JsonProperty(PropertyName = "multiplicity")]
        public Multiplicity Multiplicity { get; set; }

        [JsonProperty(PropertyName = "entityTypes")]
        public Dictionary<string, EntityType> EntityTypes { get; set; }
    }

}