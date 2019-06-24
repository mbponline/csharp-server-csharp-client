using Newtonsoft.Json;
using System.Collections.Generic;

namespace Client.Modules.Utils.DAL.Common.MetadataCli
{

    public class NavigationProperty
    {
        [JsonProperty(PropertyName = "entityTypeName")]
        public string EntityTypeName { get; set; }

        [JsonProperty(PropertyName = "multiplicity")]
        public string Multiplicity { get; set; }

        [JsonProperty(PropertyName = "keyLocal")]
        public string[] KeyLocal { get; set; }

        [JsonProperty(PropertyName = "keyRemote")]
        public string[] KeyRemote { get; set; }

        [JsonProperty(PropertyName = "annotations", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Annotations { get; set; }
    }

}