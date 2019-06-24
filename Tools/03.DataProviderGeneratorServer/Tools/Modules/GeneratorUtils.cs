using System.Collections.Generic;
using System.Linq;
using Tools.Modules.Common;
using MetadataCli = Tools.Modules.Common.MetadataCli;

namespace Tools.Modules
{

    internal static class GeneratorUtils
    {

        public static void WriteProperties(BlockWriter br, Dictionary<string, MetadataCli.Property> properties)
        {
            foreach (var property in properties)
            {
                var type = property.Value.Type;
                if (property.Value.Nullable)
                {
                    var nullable = (new string[] { "string", "object", "byte[]" }).Contains(type) ? string.Empty : "?";
                    br.WriteLine(string.Format("public {0}{2} {1} {{ get {{ return ({0}{2})(this.entity.dto[\"{1}\"].HasValues ? this.entity.dto[\"{1}\"] : null); }} set {{ this.entity.dto[\"{1}\"] = new JValue(value); }} }}", type, property.Key, nullable));
                }
                else
                {
                    br.WriteLine(string.Format("public {0} {1} {{ get {{ return ({0})this.entity.dto[\"{1}\"]; }} set {{ this.entity.dto[\"{1}\"] = new JValue(value); }} }}", type, property.Key));
                }
            }
            br.WriteLine();
        }

        public static void WriteNavigationProperties(BlockWriter br, string entityTypeName, Dictionary<string, MetadataCli.NavigationProperty> navigationProperties)
        {
            foreach (var navigationProperty in navigationProperties)
            {
                var anp = navigationProperty.Value;
                var multi = anp.Multiplicity == "multi";
                var returnType = multi ? string.Format("IEnumerable<{0}>", anp.EntityTypeName) : anp.EntityTypeName;
                var navigationType = multi ? "Multi" : "Single";

                br.WriteLine("[JsonIgnore]");
                if (multi)
                {
                    br.WriteLine(string.Format("public {0} {1} {{ get {{ return this.entity.Navigate{2}<{3}>(\"{4}\", \"{1}\"); }} }}", returnType, navigationProperty.Key, navigationType, anp.EntityTypeName, entityTypeName));
                }
                else
                {
                    br.WriteLine(string.Format("public {0} {1} {{ get {{ return this.entity.Navigate{2}<{0}>(\"{4}\", \"{1}\"); }} }}", returnType, navigationProperty.Key, navigationType, anp.EntityTypeName, entityTypeName));
                }
            }
            br.WriteLine();
        }
    }

}