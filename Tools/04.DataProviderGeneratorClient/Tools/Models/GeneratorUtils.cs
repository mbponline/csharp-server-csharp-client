using CodeGenerator.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Models
{

    internal static class GeneratorUtils
    {
        public static void WriteServiceOperationsProperties(BlockWriter br, string operationTypeName, IEnumerable<Operation> operations, Dictionary<string, string> opTypeConvert)
        {
            foreach (var op in operations)
            {
                var parametersAsList = GeneratorUtils.GetOperationParamList(op.Parameters, "{0} {1}", opTypeConvert);
                var parametersAsDictionary = GeneratorUtils.GetOperationParamList(op.Parameters, "{{ \"{0}\", {1} }}", opTypeConvert);
                switch (operationTypeName)
                {
                    case "functions":
                        if (op.ReturnType != null && op.ReturnType.IsEntity && op.ReturnType.IsCollection)
                        {
                            br.WriteLine(string.Format("public async Task<QueryResult<{0}>> {1}({2}, QueryObject queryObject = null) {{ return await this.GetEntitiesAsync<{0}>(\"{1}\", new Dictionary<string, object>() {{ {3} }}, queryObject); }}", op.ReturnType.Type, op.Name, parametersAsList, parametersAsDictionary));
                        }
                        else if (op.ReturnType != null && op.ReturnType.IsEntity && !op.ReturnType.IsCollection)
                        {
                            br.WriteLine(string.Format("public async Task<{0}> {1}({2}) {{ return await this.GetSingleEntityAsync<{0}>(\"{1}\", new Dictionary<string, object>() {{ {3} }}); }}", op.ReturnType.Type, op.Name, parametersAsList, parametersAsDictionary));
                        }
                        else
                        {
                            throw new Exception("invalid function description");
                        }
                        break;
                    case "actions":
                        if (op.ReturnType == null)
                        {
                            br.WriteLine(string.Format("public async Task {0}({1}) {{ await this.PostOperationAsync<object>(\"{0}\", new Dictionary<string, object>() {{ {2} }}); }}", op.Name, parametersAsList, parametersAsDictionary));
                        }
                        else if (op.ReturnType != null && !op.ReturnType.IsEntity)
                        {
                            br.WriteLine(string.Format("public async Task<{0}> {1}({2}) {{ return await this.PostOperationAsync<{0}>(\"{1}\", new Dictionary<string, object>() {{ {3} }}); }}", op.ReturnType.Type, op.Name, parametersAsList, parametersAsDictionary));
                        }
                        else if (op.ReturnType != null && op.ReturnType.IsEntity && op.ReturnType.IsCollection)
                        {
                            br.WriteLine(string.Format("public async Task<IEnumerable<{0}>> {1}({2}) {{ return await this.GetEntitiesPostOperationAsync<{0}>(\"{1}\", new Dictionary<string, object>() {{ {3} }}); }}", op.ReturnType.Type, op.Name, parametersAsList, parametersAsDictionary));
                        }
                        else if (op.ReturnType != null && op.ReturnType.IsEntity && !op.ReturnType.IsCollection)
                        {
                            br.WriteLine(string.Format("public async Task<{0}> {1}({2}) {{ return await this.GetEntityPostOperationAsync<{0}>(\"{1}\", new Dictionary<string, object>() {{ {3} }}); }}", op.ReturnType, op.Name, parametersAsList, parametersAsDictionary));
                        }
                        else
                        {
                            throw new Exception("invalid action description");
                        }
                        break;
                    default:
                        throw new Exception("invalid operation type name");
                }
            }

        }

        private static string GetOperationParamList(Parameter[] parameters, string template, Dictionary<string, string> opTypeConvert)
        {
            var result = parameters.Select((it) => string.Format(template, opTypeConvert[it.Type], it.Name)).ToList();
            return string.Join(", ", result);
        }

		/*
        public static void WriteDefaultValues(BlockWriter br, Dictionary<string, Property> etp)
        {
            foreach (var property in etp)
            {
                br.WriteLine(string.Format("this.{0} = {1};", property.Key, GetDefaultValue(property.Value.Default)));
            }
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
		*/

		public static void WriteProperties(BlockWriter br, Dictionary<string, Property> etp, Dictionary<string, string> dbTypeConvert)
        {
            foreach (var property in etp)
            {
                var nullable = property.Value.Nullable ? "?" : string.Empty;
                var type = dbTypeConvert[property.Value.Type];
                nullable = (new string[] { "string", "object", "byte[]" }).Contains(type) ? string.Empty : nullable;
                var integers = new List<string>() { "int", "short", "sbyte", "ushort" };
                // Info credit: http://geekswithblogs.net/BlackRabbitCoder/archive/2011/01/27/c.net-little-pitfalls-the-dangers-of-casting-boxed-values.aspx
                br.WriteLine(string.Format("public {0}{1} {2} {{ get {{ return ({0}{1}){3}this.entity.dto[\"{2}\"]; }} set {{ this.entity.dto[\"{2}\"] = value; }} }}", type, nullable, property.Key, integers.Contains(type) ? "(long)" : string.Empty));
            }
            br.WriteLine();
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
                br.WriteLine(string.Format("public {0} {1} {{ get {{ return this.entity.Navigate{2}<{3}>(\"{4}\", \"{1}\"); }} }}", returnType, navigationProperty.Key, navigationType, anp.EntityTypeName, entityTypeName));
            }
            br.WriteLine();
        }
    }
}