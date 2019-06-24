using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DatabaseTypes = Tools.Modules.Common.Database.Types;
using MetadataSrv = Tools.Modules.Common.MetadataSrv;

namespace Tools.Modules
{

    public static class GeneratorUtils
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
            MatchCollection matches;

            matches = Regex.Matches(tableName, @"(\w+i)ves$");
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].Value + "fe";
            }

            matches = Regex.Matches(tableName, @"(\w+)ves$");
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].Value + "f";
            }

            matches = Regex.Matches(tableName, @"(\w+[^aeiou])ies$");
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].Value + "y";
            }

            matches = Regex.Matches(tableName, @"(\w+(o|sh|x|ch|ss|s))es$");
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].Value;
            }

            if (Regex.IsMatch(tableName, @"\w+ss$"))
            {
                return tableName;
            }

            matches = Regex.Matches(tableName, @"(\w+)s$");
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].Value;
            }

            return tableName;
        }

        // Info util: [7 Plural Spelling Rules](https://howtospell.co.uk/pluralrules.php)
        // Info util: [Regular Expressions (Regex) Tutorial](https://www.youtube.com/watch?v=sa-TUpSx1JA)
        public static string Pluralize(this string tableName)
        {
            MatchCollection matches;

            if (Regex.IsMatch(tableName, @"\w+(ch|s|sh|x|z)$"))
            {
                return tableName + "es";
            }

            if (!Regex.IsMatch(tableName, @"\w+ff$"))
            {
                matches = Regex.Matches(tableName, @"(\w+)(f|fe)$");
                if (matches.Count > 0)
                {
                    return matches[0].Groups[1].Value + "ves";
                }
            }

            matches = Regex.Matches(tableName, @"(\w+[^aeiou])y$");
            if (matches.Count > 0)
            {
                return matches[0].Groups[1].Value + "ies";
            }

            if (Regex.IsMatch(tableName, @"\w+[^aeiou]o$"))
            {
                return tableName + "es";
            }

            return tableName + "s";
        }

        public static string GetNavigationPropertyName(this Dictionary<string, MetadataSrv.NavigationProperty> navigationProperties, string proposedName)
        {
            var result = proposedName;
            if (navigationProperties.ContainsKey(proposedName))
            {
                result = IncrementNumberedString(proposedName);
                result = navigationProperties.GetNavigationPropertyName(result);
            }
            return result;
        }

        public static string IncrementNumberedString(string str)
        {
            var result = str;
            var matches = Regex.Matches(str, @"(\w+?0*)(\d+)$");
            if (matches.Count > 0)
            {
                var n = int.Parse(matches[0].Groups[2].Value);
                result = matches[0].Groups[1].Value + (n + 1).ToString();
            }
            else
            {
                result = str + "1";
            }
            return result;
        }

        public static bool ToBoolean(this int val)
        {
            return val <= 0 ? false : true;
        }

        public static object GetDefaultValue(DatabaseTypes.Column column)
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

    }

}
