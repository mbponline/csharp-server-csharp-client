using System.Collections.Generic;
using System.Linq;
using Tools.Modules.Common.Database;
using DatabaseTypes = Tools.Modules.Common.Database.Types;
using MetadataSrv = Tools.Modules.Common.MetadataSrv;

namespace Tools.Modules
{
    public static class Generator
    {
        public static MetadataSrv.Metadata Generate()
        {
            var tableSchema = "sakila";

            var db = new DatabaseOperations("MYSQL", $"Server=localhost;Database={tableSchema};Uid=root;Pwd=Pass@word1;");

            var descriptionsSql = "SELECT @@VERSION as Version"; // SELECT VERSION() AS Version;
            var descriptions = db.Query<DatabaseTypes.Description>(descriptionsSql);

            var tablesSql = string.Format("SELECT TABLE_NAME AS Name FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME <> 'sysdiagrams' AND TABLE_SCHEMA = '{0}' AND TABLE_TYPE != 'VIEW';", tableSchema);
            var tables = db.Query<DatabaseTypes.Table>(tablesSql);

            var columnsSql = string.Format("SELECT c.TABLE_NAME AS 'Table', c.COLUMN_NAME AS Name, CASE c.COLUMN_TYPE WHEN 'tinyint(1)' THEN 'bit' ELSE c.DATA_TYPE END AS Type, c.COLUMN_DEFAULT AS 'Default', c.CHARACTER_MAXIMUM_LENGTH AS MaxLength, CASE c.IS_NULLABLE WHEN 'NO' THEN 0 WHEN 'YES' THEN 1 END AS IsNullable, CASE WHEN cu.COLUMN_NAME IS NULL THEN 0 ELSE 1 END AS IsKey, CASE c.EXTRA WHEN 'auto_increment' THEN 1 ELSE 0 END AS IsIdentity FROM INFORMATION_SCHEMA.COLUMNS AS c LEFT JOIN (SELECT t.TABLE_SCHEMA, t.TABLE_NAME, k.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS t LEFT JOIN INFORMATION_SCHEMA.key_column_usage AS k USING(constraint_name, table_schema, table_name) WHERE t.CONSTRAINT_TYPE='PRIMARY KEY') AS cu ON cu.TABLE_SCHEMA = c.TABLE_SCHEMA AND cu.TABLE_NAME = c.TABLE_NAME AND cu.COLUMN_NAME = c.COLUMN_NAME WHERE c.TABLE_SCHEMA = '{0}' ORDER BY c.TABLE_NAME, c.ORDINAL_POSITION;", tableSchema);
            var allColumns = db.Query<DatabaseTypes.Column>(columnsSql);

            var relationsSql = string.Format("SELECT cu.CONSTRAINT_NAME AS ForeignKeyName, cu.TABLE_NAME AS ParentTable, cu.COLUMN_NAME AS ParentColumnName, c.ORDINAL_POSITION AS ParentColumnId, cu.REFERENCED_TABLE_NAME AS ReferencedTable, cu.REFERENCED_COLUMN_NAME AS ReferencedColumnName FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS cu INNER JOIN INFORMATION_SCHEMA.COLUMNS AS c ON cu.TABLE_SCHEMA = c.TABLE_SCHEMA AND cu.TABLE_NAME = c.TABLE_NAME AND cu.COLUMN_NAME = c.COLUMN_NAME WHERE cu.TABLE_SCHEMA = '{0}' AND cu.REFERENCED_TABLE_NAME IS NOT NULL ORDER BY c.ORDINAL_POSITION;", tableSchema);
            var allRelations = db.Query<DatabaseTypes.Relation>(relationsSql);

            var originalTableName = new Dictionary<string, string>();

            // corect table names everywere
            foreach (var tbl in tables)
            {
                var transformedTableName = tbl.Name.CamelCase();
                originalTableName[transformedTableName] = tbl.Name;
                tbl.Name = transformedTableName;
            }

            // corect table names everywere
            foreach (var col in allColumns)
            {
                col.Table = col.Table.CamelCase();
            }

            // corect table names everywere
            foreach (var rel in allRelations)
            {
                rel.ParentTable = rel.ParentTable.CamelCase();
                rel.ReferencedTable = rel.ReferencedTable.CamelCase();
            }

            var entityTypes = new Dictionary<string, MetadataSrv.EntityType>();

            var tableNames = new Dictionary<string, string>();

            foreach (var table in tables)
            {
                var entityTypeName = table.Name.Singularize();
                var entitySetName = entityTypeName.Pluralize();

                tableNames.Add(entityTypeName, table.Name);

                var tableName = originalTableName[table.Name];

                var columns = allColumns.Where((it) => it.Table == table.Name);

                var key = (from t in columns where t.IsKey.ToBoolean() select t.Name.CamelCase()).ToArray();

                var properties = (from t in columns select t).ToDictionary(col => col.Name.CamelCase(), col => new MetadataSrv.Property()
                {
                    FieldName = col.Name,
                    Type = col.Type,
                    Nullable = col.IsNullable.ToBoolean(),
                    Default = GeneratorUtils.GetDefaultValue(col),
                    MaxLength = (int?)col.MaxLength
                });

                var calculatedProperties = (from t in columns where t.IsComputed.ToBoolean() || t.IsIdentity.ToBoolean() select t.Name.CamelCase()).ToArray();

                var entityType = new MetadataSrv.EntityType()
                {
                    TableName = tableName,
                    EntitySetName = entitySetName,
                    Key = key,
                    Properties = properties,
                    CalculatedProperties = calculatedProperties,
                    NavigationProperties = new Dictionary<string, MetadataSrv.NavigationProperty>(),
                };

                entityTypes.Add(entityTypeName, entityType);
            }

            foreach (var et in entityTypes)
            {
                var entityTypeName = et.Key;
                var entityType = et.Value;

                var relationsCorrected = allRelations.Where((it) => it.ParentTable == tableNames[entityTypeName]);

                var relations = from t in relationsCorrected
                                group t by t.ForeignKeyName into g
                                select new
                                {
                                    foreignKey = g.Key,
                                    parentTable = g.FirstOrDefault().ParentTable.Singularize(),
                                    referencedTable = g.FirstOrDefault().ReferencedTable.Singularize(),
                                    referentialConstraints = (from r in g
                                                              select new
                                                              {
                                                                  property = r.ParentColumnName,
                                                                  referencedProperty = r.ReferencedColumnName
                                                              }).ToList()
                                };

                var proposedName = string.Empty;

                foreach (var relation in relations)
                {
                    var keyLocal = relation.referentialConstraints.Select((it) => it.property.CamelCase()).ToArray();
                    var keyRemote = relation.referentialConstraints.Select((it) => it.referencedProperty.CamelCase()).ToArray();

                    proposedName = entityType.NavigationProperties.GetNavigationPropertyName(relation.referencedTable);
                    entityType.NavigationProperties.Add(proposedName, new MetadataSrv.NavigationProperty()
                    {
                        EntityTypeName = relation.referencedTable,
                        Multiplicity = "single",
                        KeyLocal = keyLocal,
                        KeyRemote = keyRemote
                    });

                    var entityTypeReferenced = entityTypes[relation.referencedTable];
                    proposedName = entityTypeReferenced.NavigationProperties.GetNavigationPropertyName(relation.parentTable.Pluralize());
                    entityTypeReferenced.NavigationProperties.Add(proposedName, new MetadataSrv.NavigationProperty()
                    {
                        EntityTypeName = relation.parentTable,
                        Multiplicity = "multi",
                        KeyLocal = keyRemote,
                        KeyRemote = keyLocal
                    });
                }
            }

            var description = descriptions.FirstOrDefault().Version.Split(new char[] { '(', '-' }).FirstOrDefault().Trim();

            var metadataSrv = new MetadataSrv.Metadata
            {
                Dialect = "MYSQL",
                Version = "v0.0.1",
                Description = description,
                Namespace = "Server.Models.DataAccess",
                Multiplicity = new MetadataSrv.Multiplicity()
                {
                    Multi = "multi",
                    Single = "single"
                },
                EntityTypes = entityTypes
            };

            return metadataSrv;
        }
    }
}
