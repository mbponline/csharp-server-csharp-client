using System.Collections.Generic;

namespace NavyBlueEntities
{
	public class LocalEntityViewsBase : Dictionary<string, object>
	{
		private readonly DataContext dataContext;

		public LocalEntityViewsBase(DataContext dataContext)
		{
			this.dataContext = dataContext;
		}

		protected DataViewLocalEntity<T> GetPropertyValue<T>(/*string entityTypeName*/)
			where T : class, IDerivedEntity
		{
			var entityTypeName = typeof(T).Name;
			DataViewLocalEntity<T> instance;
			if (this.ContainsKey(entityTypeName))
			{
				instance = (DataViewLocalEntity<T>)this[entityTypeName];
			}
			else
			{
				instance = new DataViewLocalEntity<T>(entityTypeName, this.dataContext);
				this[entityTypeName] = instance;
			}
			return instance;
		}
	}
}
