using Newtonsoft.Json;
using System.Collections.Generic;

namespace Client.Models.Utils.DAL.Common
{

    public class EntityType
    {
        [JsonProperty(PropertyName = "tableName")]
        public string TableName { get; set; }

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
    }

}