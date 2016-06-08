using Newtonsoft.Json;

namespace Client.Models.Utils.DAL.Common
{

    public class ResultSerialResponse
    {
        [JsonProperty(PropertyName = "nextLink")]
        public string NextLink { get; set; }

        [JsonProperty(PropertyName = "data")]
        public ResultSerialData Data { get; set; }
    }

}