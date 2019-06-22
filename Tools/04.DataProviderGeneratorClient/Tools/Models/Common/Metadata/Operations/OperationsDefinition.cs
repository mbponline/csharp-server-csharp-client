using Newtonsoft.Json;

namespace CodeGenerator.Models.Common
{

    public class OperationsDefinition
    {
        [JsonProperty(PropertyName = "functions")]
        public Operation[] Functions { get; set; }

        [JsonProperty(PropertyName = "actions")]
        public Operation[] Actions { get; set; }
    }

}