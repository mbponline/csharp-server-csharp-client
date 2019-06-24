using Newtonsoft.Json;
using System.Collections.Generic;

namespace Client.Modules.Utils.DAL.Common.MetadataCli
{

    public class Property
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "nullable")]
        public bool Nullable { get; set; }

        [JsonProperty(PropertyName = "default")]
        public object Default { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }

        [JsonProperty(PropertyName = "annotations", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Annotations { get; set; }
    }

}