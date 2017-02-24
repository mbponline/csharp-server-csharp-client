﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models.Utils.DAL.Common
{

    public class DataViewRemote<T>
        where T : class, IDerivedEntity
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

        public async Task<T> GetSingleItemAsync(Dto partialDto, string[] expand = null)
        {
            //BusyIndicator.instance.start();
            var resultSingleSerialData = await this.dataAdapter.LoadOneAsync(this.entityTypeName, partialDto, expand);
            //BusyIndicator.instance.stop();
            var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSingleSerialData);
            return (T)derivedEntity;
        }

        public async Task<IEnumerable<T>> GetMultipleItemsAsync(Dto[] partialDtos, string[] expand = null)
        {
            //BusyIndicator.instance.start();
            var resultSerialData = await this.dataAdapter.LoadManyAsync(this.entityTypeName, partialDtos, expand);
            //BusyIndicator.instance.stop();
            var derivedEntityList = this.dataContext.AttachEntities(resultSerialData).Select(it => (T)it);
            return derivedEntityList;
        }

        public async Task<T> InsertItemAsync(Dto dto)
        {
            var keyNames = this.metadata.EntityTypes[this.entityTypeName].Key;
            var dataOriginal = this.dataContext.CreateItemDetached<T>(this.entityTypeName);
            var patchItem = this.GetPatchItemAsync(keyNames, dto, dataOriginal.entity.dto);
            //BusyIndicator.instance.start();
            var resultSingleSerialData = await this.dataAdapter.PostItemAsync(this.entityTypeName, dto);
            //BusyIndicator.instance.stop();
            var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSingleSerialData);
            return (T)derivedEntity;
        }

        public async Task<IEnumerable<T>> InsertItemsAsync(Dto[] dtos)
        {
            if (dtos.Length == 0)
            {
                return Enumerable.Empty<T>();
            }

            var derivedEntityList = new List<T>();
            //BusyIndicator.instance.start();
            var resultSingleSerialDataList = await this.dataAdapter.PostItemsAsync(this.entityTypeName, dtos);
            //BusyIndicator.instance.stop();
            foreach (var resultSingleSerialData in resultSingleSerialDataList)
            {
                var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSingleSerialData);
                derivedEntityList.Add((T)derivedEntity);
            }
            return derivedEntityList;
        }

        public async Task<T> UpdateItemAsync(Dto partialDto)
        {
            var dataOriginal = this.dataContext.entitySets[this.entityTypeName].FindByKey(partialDto);
            // aplica modificarile datelor aflate in DataContext
            DalUtils.Extend(dataOriginal.entity.dto, partialDto);
            //BusyIndicator.instance.start();
            var resultSingleSerialData = await this.dataAdapter.PutItemAsync(this.entityTypeName, dataOriginal.entity.dto);
            //BusyIndicator.instance.stop();
            var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSingleSerialData);
            return (T)derivedEntity;
        }

        public async Task<IEnumerable<T>> UpdateItemsAsync(Dto[] partialDtos)
        {
            var keyNames = this.metadata.EntityTypes[this.entityTypeName].Key;
            var dtos = new List<Dto>();
            foreach (var partialDto in partialDtos)
            {
                var dataOriginal = this.dataContext.entitySets[this.entityTypeName].FindByKey(partialDto);
                // creeaza un nou obiect ce va cuprinde doar campurile modificate
                var patchItem = this.GetPatchItemAsync(keyNames, partialDto, dataOriginal.entity.dto);
                dtos.Add(new Dto() {
                    { "patchItem", patchItem },
                    { "partialDto", partialDto }
                });
                DalUtils.Extend(dataOriginal.entity.dto, partialDto);
            }

            var derivedEntityList = new List<T>();
            //BusyIndicator.instance.start();
            var resultSingleSerialDataList = await this.dataAdapter.PatchItemsAsync(this.entityTypeName, dtos.ToArray());
            //BusyIndicator.instance.stop();
            foreach (var dataDto in resultSingleSerialDataList)
            {
                var derivedEntity = this.dataContext.AttachSingleEntitiy(dataDto);
                derivedEntityList.Add((T)derivedEntity);
            }
            return derivedEntityList;
        }

        public async Task DeleteItemAsync(Dto partialDto)
        {
            var entitySet = (IEntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
            var dataOriginal = entitySet.FindByKey(partialDto);
            entitySet.DeleteEntity(dataOriginal);
            await this.dataAdapter.DeleteItemAsync(this.entityTypeName, partialDto);
        }

        public async Task DeleteItemsAsync(Dto[] partialDtos)
        {
            var entitySet = (IEntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
            foreach (var partialDto in partialDtos)
            {
                var dataOriginal = entitySet.FindByKey(partialDto);
                entitySet.DeleteEntity(dataOriginal);
            }
            await this.dataAdapter.DeleteItemsAsync(this.entityTypeName, partialDtos);
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