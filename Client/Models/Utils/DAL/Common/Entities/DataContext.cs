using System.Collections.Generic;
using System.Linq;

namespace Client.Models.Utils.DAL.Common
{
    public class DataContext
	{
		public DataContext(Metadata metadata)
		{
			this.metadata = metadata;
			this.entitySets = new Dictionary<string, EntitySet>();
		}

		private Metadata metadata;

		public Dictionary<string, EntitySet> entitySets;

		public Dictionary<string, List<Entity>> GetEntitySets()
		{
			var result = new Dictionary<string, List<Entity>>();
			foreach (var entitySet in this.entitySets)
			{
				result.Add(entitySet.Key, entitySet.Value.Items);
			}
			return result;
		}

		public IEnumerable<Entity> GetRelatedEntities(string entityTypeName, IEnumerable<Dto> dtos, string navigationPropertyName)
		{
			var navElement = this.metadata.EntityTypes[entityTypeName].NavigationProperties[navigationPropertyName];
			var remoteEntitySet = this.entitySets.ContainsKey(navElement.EntityTypeName) ? this.entitySets[navElement.EntityTypeName] : null;
			return remoteEntitySet != null ? remoteEntitySet.NavigateAllRelated(dtos, navElement.KeyLocal, navElement.KeyRemote) : Enumerable.Empty<Entity>();
		}

		public Entity CreateItemDetached(string entityTypeName)
		{
			//if (!this.entitySets.ContainsKey(entityTypeName))
			//{
			//    this.InitializeDataSet(entityTypeName);
			//}
			//var entityType = Type.GetType(this.metadata.Namespace + "." + entityTypeName);
			//var entity = Activator.CreateInstance(entityType);

			var entityType = this.metadata.EntityTypes[entityTypeName];
			var dto = new Dto();
			dto.SetDefaultValues(entityType);
			var entity = new Entity(entityTypeName, dto);
			return entity;
		}

		public void Clear()
		{
			foreach (var entitySet in this.entitySets)
			{
				entitySet.Value.DeleteAll();
			}
		}

		public void Dispose()
		{
			// se va apela inainte de incetarea utilizarii obiectului
			// pentru a evita aparitia de memory leaks si a usura activitatea GC-ului
			foreach (var entitySet in this.entitySets)
			{
				entitySet.Value.Dispose();
			}
			this.entitySets = null;
			this.metadata = null;
		}

		public IEnumerable<Entity> AttachEntities(ResultSerialData resultSerialData)
		{
			var entityTypeName = resultSerialData.EntityTypeName;
			var dataSet = this.TraverseResults(entityTypeName, resultSerialData.Items);
			this.AttachRelatedItems(resultSerialData.RelatedItems);
			return dataSet;
		}

		public Entity AttachSingleEntitiy(ResultSingleSerialData resultSingleSerialData)
		{
			var entityTypeName = resultSingleSerialData.EntityTypeName;
			var entities = this.TraverseResults(entityTypeName, new List<Dto>() { resultSingleSerialData.Item });
			this.AttachRelatedItems(resultSingleSerialData.RelatedItems);
			return entities.FirstOrDefault();
		}

		private void AttachRelatedItems(Dictionary<string, IEnumerable<Dto>> relatedItems)
		{
			if (relatedItems != null)
			{
				foreach (var item in relatedItems)
				{
					this.TraverseResults(item.Key, item.Value);
				}
			}
		}

		private IEnumerable<Entity> TraverseResults(string entityTypeName, IEnumerable<Dto> dtos)
		{
			if (!this.entitySets.ContainsKey(entityTypeName))
			{
				this.InitializeDataSet(entityTypeName);
			}
			var entities = this.ProcessEntitySet(entityTypeName, dtos);
			return entities;
		}


		private IEnumerable<Entity> ProcessEntitySet(string entityTypeName, IEnumerable<Dto> dtos)
		{
			var entities = new List<Entity>();
			foreach (var dto in dtos)
			{
				var newEntity = this.ProcessEntity(entityTypeName, dto);
				entities.Add(newEntity);
			}
			return entities;
		}

		private Entity ProcessEntity(string entityTypeName, Dto dto)
		{
			var entity = this.entitySets[entityTypeName].UpdateEntity(dto);
			return entity;
		}

		private void InitializeDataSet(string entityTypeName)
		{
			// Initializeaza EntitySet-ul precizat la momentul utilizarii (Lazy)
			var entitySet = new EntitySet(entityTypeName, this.entitySets, this.metadata);
			this.entitySets[entityTypeName] = entitySet;
		}
	}
}