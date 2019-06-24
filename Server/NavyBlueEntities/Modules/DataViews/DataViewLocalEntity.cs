using NavyBlueDtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NavyBlueEntities
{
	public class DataViewLocalEntity<T>
		where T : class, IDerivedEntity
	{
		private readonly string entityTypeName;
		private readonly DataContext dataContext;

		public DataViewLocalEntity(string entityTypeName, DataContext dataContext)
		{
			this.entityTypeName = entityTypeName;
			this.dataContext = dataContext;
		}

		public IEnumerable<T> GetItems(Func<T, bool> predicate)
		{
			var derivedEntityList = Enumerable.Empty<T>();
			if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
			{
				var entitySet = (EntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
				derivedEntityList = entitySet.Filter(predicate);
			}
			return derivedEntityList;
		}

		public IEnumerable<T> GetMultipleItems(IEnumerable<Dto> partialDtos)
		{
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
					}
				}
			}
			return derivedEntityList;
		}

		public T GetSingleItem(Func<T, bool> predicate)
		{
			var derivedEntity = default(T);
			if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
			{
				var entitySet = (EntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
				derivedEntity = entitySet.Find(predicate);
			}
			return derivedEntity;
		}

		public T GetSingleItem(Dto partialDto)
		{
			var derivedEntity = default(T);
			if (this.dataContext.entitySets.ContainsKey(this.entityTypeName))
			{
				var entitySet = (EntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
				derivedEntity = entitySet.FindByKey(partialDto /*partialEntity*/);
			}
			return derivedEntity;
		}

		public T CreateItemDetached()
		{
			var derivedEntity = this.dataContext.CreateItemDetached<T>(this.entityTypeName);
			return derivedEntity;
		}

		public void DetachItem(T derivedEntity)
		{
			var entitySet = (EntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
			    entitySet.DeleteEntity(derivedEntity);
		}

		public void DetachItems(IEnumerable<T> derivedEntityList)
		{
			var entitySet = (EntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
			foreach (var derivedEntity in derivedEntityList)
			{
				entitySet.DeleteEntity(derivedEntity);
			}
		}

		public void DetachAll()
		{
			var entitySet = (EntitySet<T>)this.dataContext.entitySets[this.entityTypeName];
			entitySet.DeleteAll();
		}
	}

}