using System.Collections.Generic;

namespace Client.Models.Utils.DAL.Common
{
	public class RemoteViewsBase : Dictionary<string, DataViewRemote>
	{
		public RemoteViewsBase(DataAdapter dataAdapter, DataContext dataContext, Metadata metadata)
		{
			this.dataAdapter = dataAdapter;
			this.dataContext = dataContext;
			this.metadata = metadata;
		}

		private readonly DataAdapter dataAdapter;
		private readonly DataContext dataContext;
		private readonly Metadata metadata;

		protected DataViewRemote GetPropertyValue(string entityTypeName)
		{
			DataViewRemote instance;
			if (this.ContainsKey(entityTypeName))
			{
				instance = this[entityTypeName];
			}
			else
			{
				instance = new DataViewRemote(entityTypeName, this.dataAdapter, this.dataContext, this.metadata);
				this[entityTypeName] = instance;
			}
			return instance;
		}
	}
}
