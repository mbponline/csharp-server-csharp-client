using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models.Utils.DAL.Common
{

    public class DataViewRemote<T>
        where T : class, IEntity
    {
        public DataViewRemote(DataAdapter dataAdapter, DataContext dataContext, Metadata metadata)
        {
            this.entityTypeName = typeof(T).Name;
            this.dataAdapter = dataAdapter;
            this.dataContext = dataContext;
            this.metadata = metadata;
        }

        private readonly string entityTypeName;
        private readonly DataAdapter dataAdapter;
        private readonly DataContext dataContext;
        private readonly Metadata metadata;

        public async Task<QueryResult<T>> GetItemsAsync(QueryObject queryObject = null)
        {
            var dataSet = new List<T>();
            var totalCount = 0;
            //BusyIndicator.instance.start();
            var resultSerialResponse = await this.dataAdapter.QueryAllAsync(this.entityTypeName, queryObject);
            var tempDataSet = this.dataContext.AttachEntities<T>(resultSerialResponse.Data);
            dataSet.AddRange(tempDataSet);
            var moreData = resultSerialResponse != null && !string.IsNullOrEmpty(resultSerialResponse.NextLink);
            while (moreData)
            {
                resultSerialResponse = await this.dataAdapter.queryAllNextAsync(resultSerialResponse.NextLink);
                tempDataSet = this.dataContext.AttachEntities<T>(resultSerialResponse.Data);
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

        public async Task<T> GetSingleItemAsync(Dto partialEntity, string[] expand = null)
        {
            //BusyIndicator.instance.start();
            var resultSingleSerialData = await this.dataAdapter.LoadOneAsync(this.entityTypeName, partialEntity, expand);
            //BusyIndicator.instance.stop();
            var entity = this.dataContext.AttachSingleEntitiy<T>(resultSingleSerialData);
            return entity;
        }

        public async Task<IEnumerable<T>> GetMultipleItemsAsync(Dto[] partialEntities, string[] expand = null)
        {
            //BusyIndicator.instance.start();
            var resultSerialData = await this.dataAdapter.LoadManyAsync(this.entityTypeName, partialEntities, expand);
            //BusyIndicator.instance.stop();
            var entities = this.dataContext.AttachEntities<T>(resultSerialData);
            return entities;
        }

        public async Task<T> InsertItemAsync(Dto entity)
        {
            var keyNames = this.metadata.EntityTypes[this.entityTypeName].Key;
            var dataOriginal = this.dataContext.CreateItemDetached<T>() as Dto;
            var patchItem = this.GetPatchItemAsync(keyNames, entity, dataOriginal);
            //BusyIndicator.instance.start();
            var resultSingleSerialData = await this.dataAdapter.PostItemAsync(this.entityTypeName, entity);
            //BusyIndicator.instance.stop();
            var result = this.dataContext.AttachSingleEntitiy<T>(resultSingleSerialData);
            return result;
        }

        public async Task<IEnumerable<T>> InsertItemsAsync(Dto[] entities)
        {
            if (entities.Length == 0)
            {
                return Enumerable.Empty<T>();
            }

            var result = new List<T>();
            //BusyIndicator.instance.start();
            var resultSingleSerialDataList = await this.dataAdapter.PostItemsAsync(this.entityTypeName, entities);
            //BusyIndicator.instance.stop();
            foreach (var resultSingleSerialData in resultSingleSerialDataList)
            {
                var entity = this.dataContext.AttachSingleEntitiy<T>(resultSingleSerialData);
                result.Add(entity);
            }
            return result;
        }

        public async Task<T> UpdateItemAsync(Dto partialEntity)
        {
            var dataOriginal = this.dataContext.entitySets[this.entityTypeName].FindByKey((IEntity)partialEntity);
            // aplica modificarile datelor aflate in DataContext
            DalUtils.Extend((Dto)dataOriginal, partialEntity);
            //BusyIndicator.instance.start();
            var resultSingleSerialData = await this.dataAdapter.PutItemAsync(this.entityTypeName, (Dto)dataOriginal);
            //BusyIndicator.instance.stop();
            var entity = this.dataContext.AttachSingleEntitiy<T>(resultSingleSerialData);
            return entity;
        }

        public async Task<IEnumerable<T>> UpdateItemsAsync(Dto[] partialEntities)
        {
            var keyNames = this.metadata.EntityTypes[this.entityTypeName].Key;
            var items = new List<Dto>();
            foreach (var partialEntity in partialEntities)
            {
                var dataOriginal = this.dataContext.entitySets[this.entityTypeName].FindByKey((IEntity)partialEntity);
                // creeaza un nou obiect ce va cuprinde doar campurile modificate
                var patchItem = this.GetPatchItemAsync(keyNames, partialEntity, (Dto)dataOriginal);
                items.Add(new Dto() {
                    { "patchItem", patchItem },
                    { "partialEntity", partialEntity }
                });
                DalUtils.Extend((Dto)dataOriginal, partialEntity);
            }

            var result = new List<T>();
            //BusyIndicator.instance.start();
            var resultSingleSerialDataList = await this.dataAdapter.PatchItemsAsync(this.entityTypeName, items.ToArray());
            //BusyIndicator.instance.stop();
            foreach (var dataDto in resultSingleSerialDataList)
            {
                var entity = this.dataContext.AttachSingleEntitiy<T>(dataDto);
                result.Add(entity);
            }
            return result;
        }

        public async Task DeleteItemAsync(Dto partialEntity)
        {
            var dataOriginal = this.dataContext.entitySets[this.entityTypeName].FindByKey((IEntity)partialEntity);
            this.dataContext.entitySets[this.entityTypeName].DeleteEntity(dataOriginal);
            await this.dataAdapter.DeleteItemAsync(this.entityTypeName, partialEntity);
        }

        public async Task DeleteItemsAsync(Dto[] partialEntities)
        {
            foreach (var partialEntity in partialEntities)
            {
                var dataOriginal = this.dataContext.entitySets[this.entityTypeName].FindByKey((IEntity)partialEntity);
                this.dataContext.entitySets[this.entityTypeName].DeleteEntity(dataOriginal);
            }
            await this.dataAdapter.DeleteItemsAsync(this.entityTypeName, partialEntities);
        }



        private Dto GetPatchItemAsync(string[] keyNames, Dto dataChanged, Dto dataOriginal)
        {
            var patchItem = new Dto();
            var hasChanges = false;
            foreach (var item in dataChanged)
            {
                hasChanges = false;
                if (dataOriginal.ContainsKey(item.Key) && dataChanged.ContainsKey(item.Key))
                {
                    if (dataOriginal[item.Key].GetType() == typeof(DateTime) && dataChanged[item.Key].GetType() == typeof(DateTime))
                    {
                        hasChanges = ((DateTime)dataOriginal[item.Key]).Date != ((DateTime)dataChanged[item.Key]).Date;
                    }
                    else
                    {
                        hasChanges = hasChanges = (dataOriginal[item.Key] != dataChanged[item.Key]);
                    }
                    if (hasChanges && keyNames.Contains(item.Key))
                    {
                        patchItem[item.Key] = dataChanged[item.Key];
                    }

                }
            }
            return patchItem;
        }

    }

}