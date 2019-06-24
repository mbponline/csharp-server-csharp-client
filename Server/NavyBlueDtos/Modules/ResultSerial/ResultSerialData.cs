using Newtonsoft.Json;
using System.Collections.Generic;

namespace NavyBlueDtos
{

    public class ResultSerialData
    {
        [JsonProperty(PropertyName = "items")]
        public IEnumerable<Dto> Items { get; set; }

        [JsonProperty(PropertyName = "entityTypeName")]
        public string EntityTypeName { get; set; }

        [JsonProperty(PropertyName = "totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty(PropertyName = "relatedItems")]
        public Dictionary<string, IEnumerable<Dto>> RelatedItems { get; set; }
    }

}