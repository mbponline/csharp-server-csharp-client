using Newtonsoft.Json;

namespace Client.Modules.Utils.DAL.Common.MetadataCli
{

    public class Multiplicity
    {
        [JsonProperty(PropertyName = "multi")]
        public string Multi { get; set; }

        [JsonProperty(PropertyName = "single")]
        public string Single { get; set; }
    }

}