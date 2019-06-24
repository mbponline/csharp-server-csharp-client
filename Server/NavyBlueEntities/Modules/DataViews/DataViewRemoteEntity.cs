using NavyBlueDtos;
using System.Collections.Generic;
using System.Linq;

namespace NavyBlueEntities
{

    public class DataViewRemoteEntity<T>
        where T : class, IDerivedEntity
    {
        private readonly string entityTypeName;
        private readonly DataViewDto dataViewDto;
        private readonly DataContext dataContext;

        public DataViewRemoteEntity(string entityTypeName, DataViewDto dataViewDto, DataContext dataContext)
        {
            this.entityTypeName = entityTypeName;
            this.dataViewDto = dataViewDto;
            this.dataContext = dataContext;
        }

        public int Count(QueryObject queryObject)
        {
            return this.dataViewDto.Count(this.entityTypeName, queryObject);
        }

        public IEnumerable<T> GetItems(QueryObject queryObject)
        {
            var resultSerialData = this.dataViewDto.GetItems(this.entityTypeName, queryObject);
            var derivedEntityList = this.dataContext.AttachEntities(resultSerialData); // as List<T>;
            return derivedEntityList.Cast<T>().ToList();
        }

        public T GetSingleItem(Dto partialDto, string[] expand = null)
        {
            var resultSingleSerialData = this.dataViewDto.GetSingleItem(this.entityTypeName, partialDto, expand);
            var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSingleSerialData);
            return (T)derivedEntity;
        }

        public IEnumerable<T> GetMultipleItems(IEnumerable<Dto> partialDtos, string[] expand = null)
        {
            var resultSerialData = this.dataViewDto.GetMultipleItems(this.entityTypeName, partialDtos, expand);
            var derivedEntityList = this.dataContext.AttachEntities(resultSerialData); // as List<T>;
            return derivedEntityList.Cast<T>().ToList();
        }

        public T InsertItem(Dto dto)
        {
            var resultSingleSerialData = this.dataViewDto.InsertItem(this.entityTypeName, dto);
            var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSingleSerialData);
            return (T)derivedEntity;
        }

        public IEnumerable<T> InsertItems(IEnumerable<Dto> dtos)
        {
            var derivedEntityList = new List<T>();
            var resultSingleSerialDataList = this.dataViewDto.InsertItems(this.entityTypeName, dtos);
            foreach (var resultSingleSerialData in resultSingleSerialDataList)
            {
                var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSingleSerialData);
                derivedEntityList.Add((T)derivedEntity);
            }
            return derivedEntityList;
        }

        public T UpdateItem(Dto partialDto)
        {
            var resultSingleSerialData = this.dataViewDto.UpdateItem(this.entityTypeName, partialDto);
            var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSingleSerialData);
            return (T)derivedEntity;
        }

        public IEnumerable<T> UpdateItems(IEnumerable<Dto> partialDtos)
        {
            var derivedEntityList = new List<T>();
            var resultSingleSerialDataList = this.dataViewDto.UpdateItems(this.entityTypeName, partialDtos);
            foreach (var resultSingleSerialData in resultSingleSerialDataList)
            {
                var derivedEntity = this.dataContext.AttachSingleEntitiy(resultSingleSerialData);
                derivedEntityList.Add((T)derivedEntity);
            }
            return derivedEntityList;
        }

        public T DeleteItem(Dto partialDto)
        {
            var resultSingleSerialData = this.dataViewDto.DeleteItem(this.entityTypeName, partialDto);
            var derivedEntity = default(T);
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var entitySet = (EntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
                derivedEntity = entitySet.FindByKey(partialDto);
                if (derivedEntity != null)
                {
                    entitySet.DeleteEntity(derivedEntity);
                }
            }
            return derivedEntity;
        }

        public IEnumerable<T> DeleteItems(IEnumerable<Dto> partialDtos)
        {
            var resultSerialData = this.dataViewDto.DeleteItems(this.entityTypeName, partialDtos);
            var derivedEntityList = new List<T>();
            var derivedEntity = default(T);
            if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
            {
                var entitySet = (EntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
                foreach (var partialDto in partialDtos)
                {
                    derivedEntity = entitySet.FindByKey(partialDto);
                    if (derivedEntity != null)
                    {
                        derivedEntityList.Add(derivedEntity);
                        entitySet.DeleteEntity(derivedEntity);
                    }
                }
            }
            return derivedEntityList;
        }
    }

}