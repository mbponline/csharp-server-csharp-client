using System;
using System.Collections.Generic;
using System.Linq;
using MetadataCli = Client.Modules.Utils.DAL.Common.MetadataCli;

namespace Client.Modules.Utils.DAL.Common
{
    public class DataContext
    {
        public DataContext(string @namespace, MetadataCli.Metadata metadataCli)
        {
            this.metadataCli = metadataCli;
            this.@namespace = @namespace;
            this.entitySets = new Dictionary<string, IEntitySet<IDerivedEntity>>();
        }

        private MetadataCli.Metadata metadataCli;
        private readonly string @namespace;

        public Dictionary<string, IEntitySet<IDerivedEntity>> entitySets;


        public Dictionary<string, List<IDerivedEntity>> GetEntitySets()
        {
            var result = new Dictionary<string, List<IDerivedEntity>>();
            foreach (var entitySet in this.entitySets)
            {
                var entitySetValue = entitySet.Value;
                result.Add(entitySet.Key, entitySetValue.Items); //.Cast<IDerivedEntity>().ToList()
            }
            return result;
        }

        public IEnumerable<IDerivedEntity> GetRelatedEntities(string entityTypeName, IEnumerable<Dto> dtos, string navigationPropertyName)
        {
            var navElement = this.metadataCli.EntityTypes[entityTypeName].NavigationProperties[navigationPropertyName];
            var remoteEntitySet = this.entitySets.ContainsKey(navElement.EntityTypeName) ? this.entitySets[navElement.EntityTypeName] : null;
            return remoteEntitySet != null ? remoteEntitySet.NavigateAllRelated(dtos, navElement.KeyLocal, navElement.KeyRemote) : Enumerable.Empty<IDerivedEntity>(); //.Cast<IDerivedEntity>().ToList()
        }

        public T CreateItemDetached<T>(string entityTypeName)
            where T : class, IDerivedEntity
        {
            //if (!this.entitySets.ContainsKey(entityTypeName))
            //{
            //    this.InitializeDataSet(entityTypeName);
            //}
            //var entityType = Type.GetType(this.metadata.Namespace + "." + entityTypeName);
            //var entity = Activator.CreateInstance(entityType);

            var entityType = this.metadataCli.EntityTypes[entityTypeName];
            var dto = new Dto();
            dto.SetDefaultValues(entityType);
            var entity = new Entity(entityTypeName, dto);
            var derivedEntityType = Type.GetType(this.@namespace + "." + entityTypeName);
            var derivedEntity = (T)Activator.CreateInstance(derivedEntityType, new object[] { entity });
            return derivedEntity;
        }

        public void Clear()
        {
            foreach (var entitySet in this.entitySets)
            {
                var entitySetValue = entitySet.Value;
                entitySetValue.DeleteAll();
            }
        }

        public void Dispose()
        {
            // se va apela inainte de incetarea utilizarii obiectului
            // pentru a evita aparitia de memory leaks si a usura activitatea GC-ului
            foreach (var entitySet in this.entitySets)
            {
                var entitySetValue = entitySet.Value;
                entitySetValue.Dispose();
            }
            this.entitySets = null;
            this.metadataCli = null;
        }

        public IEnumerable<IDerivedEntity> AttachEntities(ResultSerialData resultSerialData)
        {
            var entityTypeName = resultSerialData.EntityTypeName;
            var derivedEntityList = this.TraverseResults(entityTypeName, resultSerialData.Items);
            this.AttachRelatedItems(resultSerialData.RelatedItems);
            return derivedEntityList;
        }

        public IDerivedEntity AttachSingleEntitiy(ResultSingleSerialData resultSingleSerialData)
        {
            var entityTypeName = resultSingleSerialData.EntityTypeName;
            var derivedEntityList = this.TraverseResults(entityTypeName, new List<Dto>() { resultSingleSerialData.Item });
            this.AttachRelatedItems(resultSingleSerialData.RelatedItems);
            return derivedEntityList.FirstOrDefault();
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

        private IEnumerable<IDerivedEntity> TraverseResults(string entityTypeName, IEnumerable<Dto> dtos)
        {
            if (!this.entitySets.ContainsKey(entityTypeName))
            {
                this.InitializeDataSet(entityTypeName);
            }
            var derivedEntityList = this.ProcessEntitySet(entityTypeName, dtos);
            return derivedEntityList;
        }


        private IEnumerable<IDerivedEntity> ProcessEntitySet(string entityTypeName, IEnumerable<Dto> dtos)
        {
            var derivedEntityList = new List<IDerivedEntity>();
            foreach (var dto in dtos)
            {
                var derivedEntity = this.ProcessEntity(entityTypeName, dto);
                derivedEntityList.Add(derivedEntity);
            }
            return derivedEntityList;
        }

        private IDerivedEntity ProcessEntity(string entityTypeName, Dto dto)
        {
            var entitySetValue = this.entitySets[entityTypeName];
            var derivedEntity = entitySetValue.UpdateEntity(dto);
            return derivedEntity;
        }

        private void InitializeDataSet(string entityTypeName)
        {
            // Initializeaza EntitySet-ul precizat la momentul utilizarii (Lazy)
            this.entitySets[entityTypeName] = EntitySet<IDerivedEntity>.CreateEntitySet(entityTypeName, this.entitySets, this.@namespace, this.metadataCli);
        }
    }
}
