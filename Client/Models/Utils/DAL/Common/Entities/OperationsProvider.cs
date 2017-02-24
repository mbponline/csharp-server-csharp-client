using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Client.Models.Utils.DAL.Common
{
    public class OperationsProvider
    {
        public OperationsProvider(DataAdapter dataAdapter, DataContext dataContext, Metadata metadata)
        {
            this.dataAdapter = dataAdapter;
            this.dataContext = dataContext;
            this.metadata = metadata;
        }

        private DataAdapter dataAdapter;
        private DataContext dataContext;
        private Metadata metadata;

        public async Task<QueryResult<T>> GetEntitiesAsync<T>(string operationName, Dictionary<string, object> paramList, QueryObject queryObject)
            where T : class, IDerivedEntity
        {
            var dataSet = new List<T>();
            var totalCount = 0;
            var paramsQueryString = this.CreateParamsQueryString(paramList);
            //BusyIndicator.instance.start();
            var resultSerialResponse = await this.dataAdapter.QueryServiceOperationAsync<ResultSerialResponse>(operationName, paramsQueryString, queryObject, "GET", true);
            var tempDataSet = this.dataContext.AttachEntities(resultSerialResponse.Data).Select(it => (T)it);
            dataSet.AddRange(tempDataSet);
            var moreData = resultSerialResponse != null && !string.IsNullOrEmpty(resultSerialResponse.NextLink);
            while (moreData)
            {
                resultSerialResponse = await this.dataAdapter.queryAllNextAsync(resultSerialResponse.NextLink);
                tempDataSet = this.dataContext.AttachEntities(resultSerialResponse.Data).Select(it => (T)it);
                dataSet.AddRange(tempDataSet);
                totalCount = resultSerialResponse.Data.TotalCount;
                moreData = resultSerialResponse != null && !string.IsNullOrEmpty(resultSerialResponse.NextLink);
            }
            //BusyIndicator.instance.stop();

            var queryResult = new QueryResult<T>()
            {
                Rows = dataSet,
                TotalRows = totalCount
            };
            return queryResult;

        }

        public async Task<T> GetSingleEntityAsync<T>(string operationName, Dictionary<string, object> paramList)
            where T : class, IDerivedEntity
        {
            var paramsQueryString = this.CreateParamsQueryString(paramList);
            //BusyIndicator.instance.start();
            var resultSerialResponse = await this.dataAdapter.QueryServiceOperationAsync<ResultSingleSerialData>(operationName, paramsQueryString, null, "GET", false);
            //BusyIndicator.instance.stop();
            var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSerialResponse);
            return (T)derivedEntity;
        }


        public async Task<T> PostOperationAsync<T>(string operationName, Dictionary<string, object> paramList)
        {
            var paramsQueryString = this.CreateParamsQueryString(paramList);
            //BusyIndicator.instance.start();
            var result = await dataAdapter.QueryServiceOperationAsync<ValueResult<T>>(operationName, paramsQueryString, null, "POST", false);
            //BusyIndicator.instance.stop();
            return result.Value;
        }


        public async Task<IEnumerable<T>> GetEntitiesPostOperationAsync<T>(string operationName, Dictionary<string, object> paramList)
            where T : class, IDerivedEntity
        {
            var paramsQueryString = this.CreateParamsQueryString(paramList);
            //BusyIndicator.instance.start();
            var resultSerialData = await this.dataAdapter.QueryServiceOperationAsync<ResultSerialData>(operationName, paramsQueryString, null, "POST", true);
            //BusyIndicator.instance.stop();
            var derivedEntityList = this.dataContext.AttachEntities(resultSerialData).Select(it => (T)it);
            return derivedEntityList;
        }

        public async Task<T> GetEntityPostOperationAsync<T>(string operationName, Dictionary<string, object> paramList)
            where T : class, IDerivedEntity
        {
            var paramsQueryString = this.CreateParamsQueryString(paramList);
            //BusyIndicator.instance.start();
            var resultSingleSerialData = await this.dataAdapter.QueryServiceOperationAsync<ResultSingleSerialData>(operationName, paramsQueryString, null, "POST", true);
            //BusyIndicator.instance.stop();
            var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSingleSerialData);
            return (T)derivedEntity;
        }


        private string CreateParamsQueryString(Dictionary<string, object> paramList)
        {
            var result = new List<string>();
            foreach (var it in paramList)
            {
                if (it.Value.GetType() == typeof(bool))
                {
                    result.Add(string.Format("{0}={1}", it.Key, it.Value.ToString()));
                }
                else if (it.Value.GetType() == typeof(DateTime))
                {
                    result.Add(string.Format("{0}={1}", it.Key, it.Value.ToString()));
                }
                else
                {
                    result.Add(string.Format("${0}=${1}", it.Key, it.Value.ToString()));
                }
            }
            return string.Join("&", result).Replace("=null", "=");
        }


    }
}
