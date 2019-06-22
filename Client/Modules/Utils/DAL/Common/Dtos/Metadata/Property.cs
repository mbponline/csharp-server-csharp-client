using Newtonsoft.Json;

namespace Client.Modules.Utils.DAL.Common
{

    public class Property
    {
        [JsonProperty(PropertyName = "fieldName")]
        public string FieldName { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "nullable")]
        public bool Nullable { get; set; }

        [JsonProperty(PropertyName = "default")]
        public object Default { get; set; }

        [JsonProperty(PropertyName = "maxLength")]
        public int? MaxLength { get; set; }
    }

}