using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tools.Modules.Common.MetadataCli
{

    public class EntityType
    {
        [JsonProperty(PropertyName = "entitySetName")]
        public string EntitySetName { get; set; }

        [JsonProperty(PropertyName = "key")]
        public string[] Key { get; set; }

        [JsonProperty(PropertyName = "properties")]
        public Dictionary<string, Property> Properties { get; set; }

        [JsonProperty(PropertyName = "calculatedProperties")]
        public string[] CalculatedProperties { get; set; }

        [JsonProperty(PropertyName = "navigationProperties")]
        public Dictionary<string, NavigationProperty> NavigationProperties { get; set; }

        [JsonProperty(PropertyName = "annotations", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Annotations { get; set; }
    }

}