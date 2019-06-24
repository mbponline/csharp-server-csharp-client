using NavyBlueDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using MetadataSrv = NavyBlueDtos.MetadataSrv;

namespace NavyBlueEntities
{
    public class DataViewLocalDto<T>
        where T : class, IDerivedEntity
    {
        private readonly string entityTypeName;
        private readonly DataContext dataContext;
        private readonly MetadataSrv.Metadata metadataSrv;

        public DataViewLocalDto(string entityTypeName, DataContext dataContext, MetadataSrv.Metadata metadataSrv)
        {
            this.entityTypeName = entityTypeName;
            this.dataContext = dataContext;
            this.metadataSrv = metadataSrv;
        }

        public ResultSerialData GetItems(Func<T, bool> predicate, string[] expand)
        {
            var resultSerialData = new ResultSerialData()
            {
                Items = { },
                EntityTypeName = this.entityTypeName,
                TotalCount = 0,
                RelatedItems = { }
            };
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var entitySet = (EntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
                var derivedEntityList = entitySet.Filter(predicate);
                resultSerialData.Items = derivedEntityList.Select((it) => it.entity.dto).ToList();
                DataViewLocalDtoUtils.FillResultRelatedItems(this.entityTypeName, resultSerialData, expand, this.dataContext, this.metadataSrv);
            }
            return resultSerialData;
        }

        public ResultSerialData GetMultipleItems(IEnumerable<Dto> partialDtos, string[] expand)
        {
            var resultSerialData = new ResultSerialData()
            {
                Items = null,
                EntityTypeName = this.entityTypeName,
                TotalCount = 0,
                RelatedItems = { }
            };
            var derivedEntity = default(T);
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var dtos = new List<Dto>();
                var entitySet = (EntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
                foreach (var partialDto in partialDtos)
                {
                    derivedEntity = entitySet.FindByKey(partialDto);
                    if (derivedEntity != null)
                    {
                        dtos.Add(derivedEntity.entity.dto);
                    }
                }
                resultSerialData.Items = dtos;
                DataViewLocalDtoUtils.FillResultRelatedItems(this.entityTypeName, resultSerialData, expand, this.dataContext, this.metadataSrv);
            }
            return resultSerialData;
        }

        public ResultSingleSerialData GetSingleItem(Func<T, bool> predicate, string[] expand)
        {
            var resultSingleSerialData = new ResultSingleSerialData()
            {
                Item = null,
                EntityTypeName = this.entityTypeName,
                RelatedItems = { }
            };
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var entitySet = (EntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
                var derivedEntity = entitySet.Find(predicate);
                resultSingleSerialData.Item = derivedEntity.entity.dto;
                DataViewLocalDtoUtils.FillResultSingleRelatedItems(this.entityTypeName, resultSingleSerialData, expand, this.dataContext, this.metadataSrv);
            }
            return resultSingleSerialData;
        }

        public ResultSingleSerialData GetSingleItem1(Dto partialDto, string[] expand)
        {
            var resultSingleSerialData = new ResultSingleSerialData()
            {
                Item = null,
                EntityTypeName = this.entityTypeName,
                RelatedItems = { }
            };
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var entitySet = (EntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
                var derivedEntity = entitySet.FindByKey(partialDto /*partialEntity*/);
                resultSingleSerialData.Item = derivedEntity.entity.dto;
                DataViewLocalDtoUtils.FillResultSingleRelatedItems(this.entityTypeName, resultSingleSerialData, expand, this.dataContext, this.metadataSrv);
            }
            return resultSingleSerialData;
        }

        //public T CreateItemDetached()
        //{
        //    var item = this.dataContext.CreateItemDetached<T>(this.entityTypeName);
        //    return item;
        //}

        //public void DetachItem(T entity)
        //{
        //    this.dataContext.entitySets[this.entityTypeName].DeleteEntity(entity);
        //}

        //public void DetachItems(T[] entities)
        //{
        //    foreach (var entity in entities)
        //    {
        //        this.dataContext.entitySets[this.entityTypeName].DeleteEntity(entity);
        //    }
        //}

        //public void DetachAll()
        //{
        //    this.dataContext.entitySets[this.entityTypeName].DeleteAll();
        //}
    }

}