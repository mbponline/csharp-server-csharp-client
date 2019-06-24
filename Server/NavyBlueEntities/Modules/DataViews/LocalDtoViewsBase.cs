using System.Collections.Generic;
using MetadataSrv = NavyBlueDtos.MetadataSrv;

namespace NavyBlueEntities
{
    public class LocalDtoViewsBase : Dictionary<string, object>
    {
        private readonly DataContext dataContext;
        private readonly MetadataSrv.Metadata metadataSrv;

        public LocalDtoViewsBase(DataContext dataContext, MetadataSrv.Metadata metadataSrv)
        {
            this.dataContext = dataContext;
            this.metadataSrv = metadataSrv;
        }

        protected DataViewLocalDto<T> GetPropertyValue<T>(/*string entityTypeName*/)
            where T : class, IDerivedEntity
        {
            var entityTypeName = typeof(T).Name;
            DataViewLocalDto<T> instance;
            if (this.ContainsKey(entityTypeName))
            {
                instance = (DataViewLocalDto<T>)this[entityTypeName];
            }
            else
            {
                instance = new DataViewLocalDto<T>(entityTypeName, this.dataContext, this.metadataSrv);
                this[entityTypeName] = instance;
            }
            return instance;
        }
    }

}
