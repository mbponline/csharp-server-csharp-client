using CodeGenerator.Models.Common;
using System.Collections.Generic;
using System.Linq;

namespace MetadataGenerator.Models
{

    internal static class GeneratorUtils
    {
        public static void WriteDefaultValues(BlockWriter br, Dictionary<string, Property> etp)
        {
            foreach (var property in etp)
            {
                br.WriteLine(string.Format("this.{0} = {1};", property.Key, GetDefaultValue(property.Value.Default)));
            }
        }

        public static void WriteProperties(BlockWriter br, Dictionary<string, Property> etp, Dictionary<string, string> types)
        {
            foreach (var property in etp)
            {
                var nullable = property.Value.Nullable ? "?" : string.Empty;
                var type = types[property.Value.Type];
                nullable = (new string[] { "string", "object", "byte[]" }).Contains(type) ? string.Empty : nullable;
                br.WriteLine(string.Format("public {0}{1} {2} {{ get {{ return ({0}{1})this[\"{2}\"]; }} set {{ this[\"{2}\"] = value; }} }}", type, nullable, property.Key));
            }
            br.WriteLine();
        }

        public static string GetDefaultValue(object value)
        {
            if (value == null)
            {
                return "null";
            }
            else if (value is bool)
            {
                return value.ToString().ToLower();
            }
            else if (value is string && string.IsNullOrEmpty((string)value))
            {
                return "\"\"";
            }
            else if (value is string && (string)value == "ST_GeomFromText('POINT(0 0)')")
            {
                return "new { lat = 0, lon = 0 }";
            }
            else if (value is string && (string)value == "CURRENT_TIMESTAMP")
            {
                return "DateTime.Now";
            }
            else
            {
                return value.ToString();
            }
        }

        public static void WriteNavigationProperties(BlockWriter br, string entityTypeName, Dictionary<string, NavigationProperty> etnp)
        {
            foreach (var navigationProperty in etnp)
            {
                var anp = navigationProperty.Value;
                var multi = anp.Multiplicity == "multi";
                var returnType = multi ? string.Format("IEnumerable<{0}>", anp.EntityTypeName) : anp.EntityTypeName;
                var navigationType = multi ? "Multi" : "Single";

                br.WriteLine("[JsonIgnore]");
                br.WriteLine(string.Format("public {0} {1} {{ get {{ return this.Navigate{2}<{3}>(\"{4}\", \"{1}\"); }} }}", returnType, navigationProperty.Key, navigationType, anp.EntityTypeName, entityTypeName));
            }
            br.WriteLine();
        }
    }

}
