using Newtonsoft.Json;
using System.Collections.Generic;

namespace CodeGenerator.Models.Common
{

    public class EntityType
    {
        [JsonProperty(PropertyName = "tableName")]
        public string TableName { get; set; }

        [JsonProperty(PropertyName = "entitySetName")]
        public string EntitySetName { get; set; }

        [JsonProperty(PropertyName = "key")]
        public List<string> Key { get; set; }

        [JsonProperty(PropertyName = "properties")]
        public Dictionary<string, Property> Properties { get; set; }

        [JsonProperty(PropertyName = "calculatedProperties")]
        public List<string> CalculatedProperties { get; set; }

        [JsonProperty(PropertyName = "navigationProperties")]
        public Dictionary<string, NavigationProperty> NavigationProperties { get; set; }
    }

}