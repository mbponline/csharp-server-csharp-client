using System.Collections.Generic;

namespace Client.Models.Utils.DAL.Common
{
	public class LocalViewsBase : Dictionary<string, DataViewLocal>
	{
		public LocalViewsBase(DataContext dataContext)
		{
			this.dataContext = dataContext;
		}

		private readonly DataContext dataContext;

		protected DataViewLocal GetPropertyValue(string entityTypeName)
		{
			DataViewLocal instance;
			if (this.ContainsKey(entityTypeName))
			{
				instance = this[entityTypeName];
			}
			else
			{
				instance = new DataViewLocal(entityTypeName, this.dataContext);
				this[entityTypeName] = instance;
			}
			return instance;
		}
	}
}
