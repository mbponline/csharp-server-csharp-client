using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Client.Modules.Utils.DAL.Common
{
    public class OperationsProvider
    {
        public OperationsProvider(DataAdapter dataAdapter, DataContext dataContext)
        {
            this.dataAdapter = dataAdapter;
            this.dataContext = dataContext;
        }

        private DataAdapter dataAdapter;
        private DataContext dataContext;

        public async Task<QueryResult<T>> GetEntitiesAsync<T>(string operationName, Dictionary<string, object> paramsObject, QueryObject queryObject, string returnTypeName)
            where T : class, IDerivedEntity
        {
            var dataSet = new List<T>();
            var totalCount = 0;
            //BusyIndicator.instance.start();
            var resultSerialResponseToken = await this.dataAdapter.CallEntityFunctionAsync(operationName, paramsObject, queryObject, returnTypeName, true);
            var resultSerialResponse = resultSerialResponseToken.ToObject<ResultSerialResponse>();

            var tempDataSet = this.dataContext.AttachEntities(resultSerialResponse.Data).Select(it => (T)it);
            dataSet.AddRange(tempDataSet);
            var moreData = resultSerialResponse != null && !string.IsNullOrEmpty(resultSerialResponse.NextLink);
            while (moreData)
            {
                resultSerialResponseToken = await this.dataAdapter.QueryAllNextAsync(resultSerialResponse.NextLink);
                resultSerialResponse = resultSerialResponseToken.ToObject<ResultSerialResponse>();

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

        public async Task<T> GetSingleEntityAsync<T>(string operationName, Dictionary<string, object> paramsObject, QueryObject queryObject, string returnTypeName)
            where T : class, IDerivedEntity
        {
            //BusyIndicator.instance.start();
            var resultSerialResponseToken = await this.dataAdapter.CallEntityFunctionAsync(operationName, paramsObject, queryObject, returnTypeName, false);
            var resultSerialResponse = resultSerialResponseToken.ToObject<ResultSingleSerialData>();
            //BusyIndicator.instance.stop();
            var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSerialResponse);
            return (T)derivedEntity;
        }

        public async Task<T> GetValueAsync<T>(string functionName, Dictionary<string, object> paramsObject)
        {
            //BusyIndicator.instance.start();
            var resultToken = await this.dataAdapter.CallValueFunctionAsync(functionName, paramsObject);
            var result = resultToken.ToObject<ValueResult<T>>();
            //BusyIndicator.instance.stop();
            return result.Value;
        }

        public async Task<IEnumerable<T>> GetEntitiesPostOperationAsync<T>(string operationName, Dictionary<string, object> paramsObject, string returnTypeName)
            where T : class, IDerivedEntity
        {
            //BusyIndicator.instance.start();
            var resultSerialDataToken = await this.dataAdapter.CallEntityActionAsync(operationName, paramsObject, returnTypeName);
            var resultSerialData = resultSerialDataToken.ToObject<ResultSerialData>();
            //BusyIndicator.instance.stop();
            var derivedEntityList = this.dataContext.AttachEntities(resultSerialData).Select(it => (T)it);
            return derivedEntityList;
        }

        public async Task<T> GetSingleEntityPostOperationAsync<T>(string operationName, Dictionary<string, object> paramsObject, string returnTypeName)
            where T : class, IDerivedEntity
        {
            //BusyIndicator.instance.start();
            var resultSingleSerialDataToken = await this.dataAdapter.CallEntityActionAsync(operationName, paramsObject, returnTypeName);
            var resultSingleSerialData = resultSingleSerialDataToken.ToObject<ResultSingleSerialData>();
            //BusyIndicator.instance.stop();
            var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSingleSerialData);
            return (T)derivedEntity;
        }

        public async Task<T> GetValuePostOperationAsync<T>(string actionName, Dictionary<string, object> paramsObject)
        {
            //BusyIndicator.instance.start();
            var resultToken = await this.dataAdapter.CallValueActionAsync(actionName, paramsObject);
            var result = resultToken.ToObject<ValueResult<T>>();
            //BusyIndicator.instance.stop();
            return result.Value;
        }

    }
}
