using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MetadataCli = Client.Modules.Utils.DAL.Common.MetadataCli;

namespace Client.Modules.Utils.DAL.Common
{
    public static class MetadataUtils
    {
        public static async Task<MetadataCli.Metadata> GetMetadataAsync(string baseUrl, string metadataUrl)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(metadataUrl);
                if (response.IsSuccessStatusCode)
                {
                    var metadataString = await response.Content.ReadAsStringAsync();
                    var metadataCli = JsonConvert.DeserializeObject<MetadataCli.Metadata>(metadataString);
                    return metadataCli;
                }
                else
                {
                    throw new Exception("Could not get metadata");
                }
            };
        }

    }
}
