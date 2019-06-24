using Newtonsoft.Json;

namespace Client.Modules.Utils.DAL.Common.MetadataCli
{

    public class Parameter
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "nullable")]
        public bool Nullable { get; set; }
    }

}