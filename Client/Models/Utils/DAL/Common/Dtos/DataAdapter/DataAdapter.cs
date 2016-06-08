using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client.Models.Utils.DAL.Common
{
    public class DataAdapter
    {
        public DataAdapter(Metadata metadata, string baseUrl, string serviceUrl)
        {
            this.metadata = metadata;
            this.serviceUrl = serviceUrl;

            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(baseUrl);
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        ~DataAdapter()
        {
            this.client.Dispose();
        }

        private readonly Metadata metadata;
        private readonly string serviceUrl;
        private readonly HttpClient client;

        /**
         * Query entity collection
         */
        public async Task<ResultSerialResponse> QueryAllAsync(string entityTypeName, QueryObject queryObject)
        {
            var entitySetName = this.metadata.EntityTypes[entityTypeName].EntitySetName;
            var url = this.serviceUrl + "crud/" + entitySetName + "?" + QueryUtils.RenderQueryString(queryObject);

            var response = await this.client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var resultSerialResponse = await response.Content.ReadAsAsync<ResultSerialResponse>();
                return resultSerialResponse;
            }
            return null;
        }

        /**
         * Service operation call
         */
        public async Task<TResult> QueryServiceOperationAsync<TResult>(string operationName, string paramsQueryString, QueryObject queryObject, string httpMethod, bool returnCollection)
            where TResult : class
        {
            if (returnCollection && httpMethod == "GET")
            {
                queryObject.Count = true;
            }
            var url = this.serviceUrl + "operations/" + operationName + "?" + paramsQueryString + "&" + QueryUtils.RenderQueryString(queryObject);

            HttpResponseMessage response;
            if (httpMethod == "GET")
            {
                response = await this.client.GetAsync(url);
            }
            else
            {
                response = await this.client.PostAsJsonAsync<object>(url, null);
            }
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<TResult>();
                return result;
            }
            return null;
        }

        /**
         * Retrive a single entity
         */
        public async Task<ResultSingleSerialData> LoadOneAsync(string entityTypeName, Dto partialEntity, string[] expand)
        {
            var entitySetName = this.metadata.EntityTypes[entityTypeName].EntitySetName;
            var keyNames = this.metadata.EntityTypes[entityTypeName].Key;
            var queryObject = new QueryObject()
            {
                Keys = (new List<Dto> { DataAdapterUtils.GetKeyFromData(keyNames, partialEntity) }).ToArray(),
                Expand = expand
            };
            var url = this.serviceUrl + "crud/" + "single/" + entitySetName + "?" + QueryUtils.RenderQueryString(queryObject);

            var response = await this.client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var resultSingleSerialData = await response.Content.ReadAsAsync<ResultSingleSerialData>();
                return resultSingleSerialData;
            }
            return null;
        }

        /**
         * Retrive multiple entities
         */
        public async Task<ResultSerialData> LoadManyAsync(string entityTypeName, Dto[] partialEntities, string[] expand)
        {
            var entitySetName = this.metadata.EntityTypes[entityTypeName].EntitySetName;
            var keyNames = this.metadata.EntityTypes[entityTypeName].Key;
            var url = this.serviceUrl + "crud/" + "many/" + entitySetName + "?" + QueryUtils.RenderQueryString(new QueryObject() { Keys = DataAdapterUtils.GetKeyFromMultipleData(keyNames, partialEntities).ToArray(), Expand = expand });

            var response = await this.client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var resultSerialData = await response.Content.ReadAsAsync<ResultSerialData>();
                return resultSerialData;
            }
            return null;
        }

        /**
         * Insert single entity
         */
        public async Task<ResultSingleSerialData> PostItemAsync(string entityTypeName, Dto patchItem)
        {
            var entitySetName = this.metadata.EntityTypes[entityTypeName].EntitySetName;
            var url = this.serviceUrl + "crud/" + entitySetName;

            var response = await this.client.PostAsJsonAsync<Dto>(url, patchItem);
            if (response.IsSuccessStatusCode)
            {
                var resultSingleSerialData = await response.Content.ReadAsAsync<ResultSingleSerialData>();
                return resultSingleSerialData;
            }
            return null;
        }

        /**
        * Insert multiple entities
        */
        public async Task<List<ResultSingleSerialData>> PostItemsAsync(string entityTypeName, Dto[] entities)
        {
            var entitySetName = this.metadata.EntityTypes[entityTypeName].EntitySetName;
            var url = this.serviceUrl + "crud/" + "batch/" + entitySetName;

            var response = await this.client.PostAsJsonAsync<Dto[]>(url, entities);
            if (response.IsSuccessStatusCode)
            {
                var resultSingleSerialDataList = await response.Content.ReadAsAsync<List<ResultSingleSerialData>>();
                return resultSingleSerialDataList;
            }
            return null;
        }

        /**
         * Update single entity
         */
        public async Task<ResultSingleSerialData> PutItemAsync(string entityTypeName, Dto entity)
        {
            var entitySetName = this.metadata.EntityTypes[entityTypeName].EntitySetName;
            var keyNames = this.metadata.EntityTypes[entityTypeName].Key;
            var url = this.serviceUrl + "crud/" + entitySetName + "?" + QueryUtils.RenderQueryString(new QueryObject() { Keys = (new List<Dto>() { DataAdapterUtils.GetKeyFromData(keyNames, entity) }).ToArray() });

            var response = await this.client.PutAsJsonAsync<Dto>(url, entity);
            if (response.IsSuccessStatusCode)
            {
                var resultSingleSerialData = await response.Content.ReadAsAsync<ResultSingleSerialData>();
                return resultSingleSerialData;
            }
            return null;
        }

        /**
         * Update single entity but only the changed fields
         */
        public async Task<ResultSingleSerialData> PatchItemAsync(string entityTypeName, Dto item)
        {
            var entitySetName = this.metadata.EntityTypes[entityTypeName].EntitySetName;
            var keyNames = this.metadata.EntityTypes[entityTypeName].Key;
            var url = this.serviceUrl + "crud/" + entitySetName + "?" + QueryUtils.RenderQueryString(new QueryObject() { Keys = (new List<Dto> { DataAdapterUtils.GetKeyFromData(keyNames, (Dto)item["partialEntity"]) }).ToArray() });

            // Info credit: http://benfoster.io/blog/adding-patch-support-to-httpclient
            HttpRequestMessage request;
            using (var content = new ObjectContent<Dto>((Dto)item["patchItem"], new JsonMediaTypeFormatter()))
            {
                request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };
            }
            var response = await this.client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var resultSingleSerialData = await response.Content.ReadAsAsync<ResultSingleSerialData>();
                return resultSingleSerialData;
            }
            return null;
        }

        /**
         * Update multiple entities
         */
        public async Task<List<ResultSingleSerialData>> PatchItemsAsync(string entityTypeName, Dto[] items)
        {
            var entitySetName = this.metadata.EntityTypes[entityTypeName].EntitySetName;
            var url = this.serviceUrl + "crud/" + "batch/" + entitySetName;

            // Info credit: http://benfoster.io/blog/adding-patch-support-to-httpclient
            HttpRequestMessage request;
            using (var content = new ObjectContent<Dto[]>(items.Select((item) => (Dto)item["patchItem"]).ToArray(), new JsonMediaTypeFormatter()))
            {
                request = new HttpRequestMessage(new HttpMethod("PATCH"), url) { Content = content };
            };
            var response = await this.client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var resultSingleSerialData = await response.Content.ReadAsAsync<List<ResultSingleSerialData>>();
                return resultSingleSerialData;
            }
            return null;
        }

        /**
         * Delete single entity
         */
        public async Task<ResultSingleSerialData> DeleteItemAsync(string entityTypeName, Dto partialEntity)
        {
            var entitySetName = this.metadata.EntityTypes[entityTypeName].EntitySetName;
            var keyNames = this.metadata.EntityTypes[entityTypeName].Key;
            var url = this.serviceUrl + "crud/" + entitySetName + "?" + QueryUtils.RenderQueryString(new QueryObject() { Keys = new List<Dto>() { DataAdapterUtils.GetKeyFromData(keyNames, partialEntity) }.ToArray() });

            var response = await this.client.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var resultSingleSerialDataList = await response.Content.ReadAsAsync<ResultSingleSerialData>();
                return resultSingleSerialDataList;
            }
            return null;
        }

        /**
         * Delete multiple entities
         */
        public async Task<ResultSerialData> DeleteItemsAsync(string entityTypeName, Dto[] items)
        {
            var entitySetName = this.metadata.EntityTypes[entityTypeName].EntitySetName;
            var keyNames = this.metadata.EntityTypes[entityTypeName].Key;
            var url = this.serviceUrl + "crud/" + "batch/" + entitySetName + "?" + QueryUtils.RenderQueryString(new QueryObject() { Keys = DataAdapterUtils.GetKeyFromMultipleData(keyNames, items).ToArray() });

            var response = await this.client.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var resultSingleSerialDataList = await response.Content.ReadAsAsync<ResultSerialData>();
                return resultSingleSerialDataList;
            }
            return null;

        }

        /**
         * Query entity collection
         */
        public async Task<ResultSerialResponse> queryAllNextAsync(string url)
        {
            var response = await this.client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var resultSerialResponse = await response.Content.ReadAsAsync<ResultSerialResponse>();
                return resultSerialResponse;
            }
            return null;
        }

    }

}