using NavyBlueDtos;
using System.Collections.Generic;

namespace NavyBlueEntities
{
    public class RemoteEntityViewsBase : Dictionary<string, object>
    {
        private readonly DataViewDto dataViewDto;
        private readonly DataContext dataContext;

        public RemoteEntityViewsBase(DataViewDto dataViewDto, DataContext dataContext)
        {
            this.dataViewDto = dataViewDto;
            this.dataContext = dataContext;
        }

        protected DataViewRemoteEntity<T> GetPropertyValue<T>(/*string entityTypeName*/)
            where T : class, IDerivedEntity
        {
            var entityTypeName = typeof(T).Name;
            DataViewRemoteEntity<T> instance;
            if (this.ContainsKey(entityTypeName))
            {
                instance = (DataViewRemoteEntity<T>)this[entityTypeName];
            }
            else
            {
                instance = new DataViewRemoteEntity<T>(entityTypeName, this.dataViewDto, this.dataContext);
                this[entityTypeName] = instance;
            }
            return instance;
        }
    }
}
