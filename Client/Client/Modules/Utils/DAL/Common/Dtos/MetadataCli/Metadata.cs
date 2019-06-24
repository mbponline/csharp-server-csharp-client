using Newtonsoft.Json;
using System.Collections.Generic;

namespace Client.Modules.Utils.DAL.Common.MetadataCli
{

    public class Metadata
    {
        [JsonProperty(PropertyName = "dialect")]
        public string Dialect { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

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