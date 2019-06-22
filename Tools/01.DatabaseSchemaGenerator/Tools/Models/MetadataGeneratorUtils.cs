using CodeGenerator.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Models
{

    public static class MetadataGeneratorUtils
    {
        public static string CamelCase(this string tableOrFieldName)
        {
            var items = tableOrFieldName.Split(new char[] { '_', ' ' });
            for (var i = 0; i < items.Length; i++)
            {
                // Info credit: http://stackoverflow.com/questions/4135317/make-first-letter-of-a-string-upper-case-for-maximum-performance#answer-4405876
                items[i] = items[i].First().ToString().ToUpper() + items[i].Substring(1);
            }

            return string.Join(string.Empty, items);
        }

        public static string Singularize(this string tableName)
        {
            if (tableName.Substring(tableName.Length - 1) == "s")
            {
                if (tableName.Substring(tableName.Length - 2) == "ss")
                {
                    return tableName;
                }

                return tableName.Remove(tableName.Length - 1);
            }

            return tableName;
        }

        public static string Pluralize(this string tableName)
        {
            if (tableName.Substring(tableName.Length - 1) == "y")
            {
                return tableName.Substring(0, tableName.Length - 1) + "ies";
            }
            else if (tableName.Substring(tableName.Length - 2) == "ss")
            {
                return tableName + "es";
            }
            else
            {
                return tableName + "s";
            }
        }

        public static bool ToBoolean(this int val)
        {
            return val == 0 ? false : true;
        }

        public static object GetDefaultValue(Column column)
        {
            object defaultValue = null;
            switch (column.Type)
            {
                case "int":
                case "smallint":
                case "float":
                case "decimal":
                case "mediumint":
                case "tinyint":
                case "year":
                    defaultValue = column.IsNullable.ToBoolean() ? default(int?) : default(int); // 0
                    break;
                case "bit":
                    defaultValue = column.IsNullable.ToBoolean() ? default(bool?) : default(bool); // false
                    break;
                case "char":
                case "varchar":
                case "text":
                case "longtext":
                case "enum":
                case "set":
                    defaultValue = column.IsNullable.ToBoolean() ? null : string.Empty; // ""
                    break;
                case "datetime":
                case "timestamp":
                    defaultValue = column.IsNullable.ToBoolean() ? null : "CURRENT_TIMESTAMP";
                    break;
                case "geometry":
                    defaultValue = column.IsNullable.ToBoolean() ? null : "ST_GeomFromText('POINT(0 0)')";
                    break;
                case "blob":
                    defaultValue = column.IsNullable.ToBoolean() ? null : "[]";
                    break;
                default:
                    throw new ArgumentException("Unknown data type.");
            }

            return defaultValue;
        }

        public static string GetNavigationPropertyName(string proposedName, Dictionary<string, NavigationProperty> navigation)
        {
            if (navigation.ContainsKey(proposedName))
            {
                var last = proposedName.Substring(proposedName.Length - 1);
                int n;
                var isNumeric = int.TryParse(last, out n);
                if (isNumeric)
                {
                    return proposedName.Substring(0, proposedName.Length - 1) + (n + 1).ToString();
                }
                else
                {
                    return proposedName + "1";
                }
            }

            return proposedName;
        }
    }

}
