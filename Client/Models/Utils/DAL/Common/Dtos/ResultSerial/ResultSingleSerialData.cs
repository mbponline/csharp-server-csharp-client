using Newtonsoft.Json;
using System.Collections.Generic;

namespace Client.Models.Utils.DAL.Common
{

    public class ResultSingleSerialData
    {
        [JsonProperty(PropertyName = "item")]
        public Dto Item { get; set; }

        [JsonProperty(PropertyName = "entityTypeName")]
        public string EntityTypeName { get; set; }

        [JsonProperty(PropertyName = "relatedItems")]
        public Dictionary<string, IEnumerable<Dto>> RelatedItems { get; set; }
    }

}
