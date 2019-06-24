using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MetadataCli = Client.Modules.Utils.DAL.Common.MetadataCli;

namespace Client.Modules.Utils.DAL.Common
{
    public class DataAdapter
    {
        public DataAdapter(string baseUrl, string apiUrl, MetadataCli.Metadata metadataCli)
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(baseUrl);
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.apiUrl = apiUrl;
            this.metadataCli = metadataCli;
        }

        ~DataAdapter()
        {
            this.client.Dispose();
        }

        private readonly HttpClient client;
        private readonly string apiUrl;
        private readonly MetadataCli.Metadata metadataCli;

        /**
         * Query entity collection
         */
        public async Task<JToken> QueryAllAsync(string entityTypeName, QueryObject queryObject)
        {
            var entitySetName = this.metadataCli.EntityTypes[entityTypeName].EntitySetName;
            var url = this.apiUrl + "/crud/" + entitySetName + "?" + QueryUtils.RenderQueryString(queryObject);

            var response = await this.client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultSerialResponseToken = JToken.Parse(json);
                return resultSerialResponseToken;
            }
            return null;
        }

        /**
         * Entity Function call (a service operation that returns entities and has no side effects)
         */
        public async Task<JToken> CallEntityFunctionAsync(string functionName, Dictionary<string, object> paramsObject, QueryObject queryObject, string returnTypeName, bool returnCollection)
        {
            queryObject.Count = returnCollection;
            var paramsQueryString = DataAdapterUtils.CreateParamsQueryString(paramsObject);

            var url = this.apiUrl + "/operations/" + functionName + "?" + paramsQueryString + "&" + QueryUtils.RenderQueryString(queryObject);

            var response = await this.client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultToken = JToken.Parse(json);
                return resultToken;
            }
            return null;
        }

        /**
         * Entity Action call (a service operation that returns entities and may produce side effects)
         */
        public async Task<JToken> CallEntityActionAsync(string actionName, Dictionary<string, object> paramsObject, string returnTypeName)
        {
            var paramsQueryString = DataAdapterUtils.CreateParamsQueryString(paramsObject);

            var url = this.apiUrl + "/operations/" + actionName + "?" + paramsQueryString;

            var response = await this.client.PostAsync(url, null); ;

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultToken = JToken.Parse(json);
                return resultToken;
            }
            return null;
        }

        /**
         * Value Function call (a service operation that returns values and has no side effects)
         */
        public async Task<JToken> CallValueFunctionAsync(string functionName, Dictionary<string, object> paramsObject)
        {
            var paramsQueryString = DataAdapterUtils.CreateParamsQueryString(paramsObject);

            var url = this.apiUrl + "/operations/" + functionName + "?" + paramsQueryString;

            var response = await this.client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultToken = JToken.Parse(json);
                return resultToken;
            }
            return null;
        }

        /**
         * Value Action call (a service operation that returns values and may produce side effects)
         */
        public async Task<JToken> CallValueActionAsync(string actionName, Dictionary<string, object> paramsObject)
        {
            var url = this.apiUrl + "/operations/" + actionName;
            var paramsObjectDto = JsonConvert.SerializeObject(paramsObject);

            var response = await this.client.PostAsync(url, new StringContent(paramsObjectDto, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultToken = JToken.Parse(json);
                return resultToken;
            }
            return null;
        }


        /**
         * Retrive a single entity
         */
        public async Task<JToken> LoadOneAsync(string entityTypeName, Dto partialDto, string[] expand)
        {
            var entitySetName = this.metadataCli.EntityTypes[entityTypeName].EntitySetName;
            var keyNames = this.metadataCli.EntityTypes[entityTypeName].Key;
            var queryObject = new QueryObject()
            {
                Keys = (new List<Dto> { DataAdapterUtils.GetKeyFromData(keyNames, partialDto) }).ToArray(),
                Expand = expand
            };
            var url = this.apiUrl + "/crud/" + "single/" + entitySetName + "?" + QueryUtils.RenderQueryString(queryObject);

            var response = await this.client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultSingleSerialDataToken = JToken.Parse(json);
                return resultSingleSerialDataToken;
            }
            return null;
        }

        /**
         * Retrive multiple entities
         */
        public async Task<JToken> LoadManyAsync(string entityTypeName, Dto[] partialDtos, string[] expand)
        {
            var entitySetName = this.metadataCli.EntityTypes[entityTypeName].EntitySetName;
            var keyNames = this.metadataCli.EntityTypes[entityTypeName].Key;
            var url = this.apiUrl + "/crud/" + "many/" + entitySetName + "?" + QueryUtils.RenderQueryString(new QueryObject() { Keys = DataAdapterUtils.GetKeyFromMultipleData(keyNames, partialDtos).ToArray(), Expand = expand });

            var response = await this.client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultSerialDataToken = JToken.Parse(json);
                return resultSerialDataToken;
            }
            return null;
        }

        /**
         * Insert single entity
         */
        public async Task<JToken> PostItemAsync(string entityTypeName, Dto patchItem)
        {
            var entitySetName = this.metadataCli.EntityTypes[entityTypeName].EntitySetName;
            var url = this.apiUrl + "/crud/" + entitySetName;

            var jsonPatchItem = JsonConvert.SerializeObject(patchItem);
            var response = await this.client.PostAsync(url, new StringContent(jsonPatchItem, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultSingleSerialDataToken = JToken.Parse(json);
                return resultSingleSerialDataToken;
            }
            return null;
        }

        /**
        * Insert multiple entities
        */
        public async Task<JToken> PostItemsAsync(string entityTypeName, Dto[] entities)
        {
            var entitySetName = this.metadataCli.EntityTypes[entityTypeName].EntitySetName;
            var url = this.apiUrl + "/crud/" + "batch/" + entitySetName;

            var jsonPatchItem = JsonConvert.SerializeObject(entities);
            var response = await this.client.PostAsync(url, new StringContent(jsonPatchItem, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultSingleSerialDataListToken = JToken.Parse(json);
                return resultSingleSerialDataListToken;
            }
            return null;
        }

        /**
         * Update single entity
         */
        public async Task<JToken> PutItemAsync(string entityTypeName, Dto entity)
        {
            var entitySetName = this.metadataCli.EntityTypes[entityTypeName].EntitySetName;
            var keyNames = this.metadataCli.EntityTypes[entityTypeName].Key;
            var url = this.apiUrl + "/crud/" + entitySetName + "?" + QueryUtils.RenderQueryString(new QueryObject() { Keys = (new List<Dto>() { DataAdapterUtils.GetKeyFromData(keyNames, entity) }).ToArray() });

            var jsonPatchItem = JsonConvert.SerializeObject(entity);
            var response = await this.client.PutAsync(url, new StringContent(jsonPatchItem, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultSingleSerialDataToken = JToken.Parse(json);
                return resultSingleSerialDataToken;
            }
            return null;
        }

        /**
         * Update single entity but only the changed fields
         */
        public async Task<JToken> PatchItemAsync(string entityTypeName, Dto item)
        {
            var entitySetName = this.metadataCli.EntityTypes[entityTypeName].EntitySetName;
            var keyNames = this.metadataCli.EntityTypes[entityTypeName].Key;
            var url = this.apiUrl + "/crud/" + entitySetName + "?" + QueryUtils.RenderQueryString(new QueryObject() { Keys = (new List<Dto> { DataAdapterUtils.GetKeyFromData(keyNames, (Dto)item["partialEntity"]) }).ToArray() });

            // Info credit: http://benfoster.io/blog/adding-patch-support-to-httpclient
            HttpRequestMessage request;
            var jsonPatchItem = JsonConvert.SerializeObject(item["patchItem"]);
            using (var content = new StringContent(jsonPatchItem, Encoding.UTF8, "application/json"))
            {
                request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };
            }
            var response = await this.client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultSingleSerialDataToken = JToken.Parse(json);
                return resultSingleSerialDataToken;
            }
            return null;
        }

        /**
         * Update multiple entities
         */
        public async Task<JToken> PatchItemsAsync(string entityTypeName, Dto[] items)
        {
            var entitySetName = this.metadataCli.EntityTypes[entityTypeName].EntitySetName;
            var url = this.apiUrl + "/crud/" + "batch/" + entitySetName;

            // Info credit: http://benfoster.io/blog/adding-patch-support-to-httpclient
            HttpRequestMessage request;
            var jsonPatchItems = JsonConvert.SerializeObject(items.Select((item) => (Dto)item["patchItem"]).ToArray());
            using (var content = new StringContent(jsonPatchItems, Encoding.UTF8, "application/json"))
            {
                request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };
            };
            var response = await this.client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultSingleSerialDataListToken = JToken.Parse(json);
                return resultSingleSerialDataListToken;
            }
            return null;
        }

        /**
         * Delete single entity
         */
        public async Task<JToken> DeleteItemAsync(string entityTypeName, Dto partialEntity)
        {
            var entitySetName = this.metadataCli.EntityTypes[entityTypeName].EntitySetName;
            var keyNames = this.metadataCli.EntityTypes[entityTypeName].Key;
            var url = this.apiUrl + "/crud/" + entitySetName + "?" + QueryUtils.RenderQueryString(new QueryObject() { Keys = new List<Dto>() { DataAdapterUtils.GetKeyFromData(keyNames, partialEntity) }.ToArray() });

            var response = await this.client.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultSingleSerialDataToken = JToken.Parse(json);
                return resultSingleSerialDataToken;
            }
            return null;
        }

        /**
         * Delete multiple entities
         */
        public async Task<JToken> DeleteItemsAsync(string entityTypeName, Dto[] items)
        {
            var entitySetName = this.metadataCli.EntityTypes[entityTypeName].EntitySetName;
            var keyNames = this.metadataCli.EntityTypes[entityTypeName].Key;
            var url = this.apiUrl + "/crud/" + "batch/" + entitySetName + "?" + QueryUtils.RenderQueryString(new QueryObject() { Keys = DataAdapterUtils.GetKeyFromMultipleData(keyNames, items).ToArray() });

            var response = await this.client.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultSerialDataToken = JToken.Parse(json);
                return resultSerialDataToken;
            }
            return null;

        }

        /**
         * Query entity collection
         */
        public async Task<JToken> QueryAllNextAsync(string url)
        {
            var response = await this.client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var resultSerialResponseToken = JToken.Parse(json);
                return resultSerialResponseToken;
            }
            return null;
        }

    }

}
