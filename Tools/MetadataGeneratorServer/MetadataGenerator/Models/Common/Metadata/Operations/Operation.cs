using Newtonsoft.Json;

namespace CodeGenerator.Models.Common
{

    public class Operation
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "parameters")]
        public Parameter[] Parameters { get; set; }

        [JsonProperty(PropertyName = "returnType")]
        public ReturnType ReturnType { get; set; }
    }

}