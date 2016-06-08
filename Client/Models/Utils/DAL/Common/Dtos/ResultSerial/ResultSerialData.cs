using Newtonsoft.Json;
using System.Collections.Generic;

namespace Client.Models.Utils.DAL.Common
{

    public class ResultSerialData
    {
        [JsonProperty(PropertyName = "items")]
        public IEnumerable<object> Items { get; set; }

        [JsonProperty(PropertyName = "entityTypeName")]
        public string EntityTypeName { get; set; }

        [JsonProperty(PropertyName = "totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty(PropertyName = "relatedItems")]
        public Dictionary<string, IEnumerable<object>> RelatedItems { get; set; }
    }

}