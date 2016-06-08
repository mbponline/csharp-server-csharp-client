using Newtonsoft.Json;

namespace Server.Models.Utils.DAL.Common
{

    public class OperationsDefinition
    {
        [JsonProperty(PropertyName = "functions")]
        public Operation[] Functions { get; set; }

        [JsonProperty(PropertyName = "actions")]
        public Operation[] Actions { get; set; }
    }

}