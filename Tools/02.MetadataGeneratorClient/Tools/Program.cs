using Newtonsoft.Json;
using System;
using System.IO;
using Tools.Modules;
using MetadataCli = Tools.Modules.Common.MetadataCli;

namespace Tools
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var pathToAppData = ProgramUtils.GetPathToAppData();

            var metadataSrv = pathToAppData.GetJsonFromAppDataAs<MetadataCli.Metadata>("metadata_srv.json");
            var metadataCliOperations = pathToAppData.GetJsonFromAppDataAs<MetadataCli.Metadata>("metadata_cli_operations.json");

            var metadataCliFull = Generator.GenerateMetadataCliFull(metadataSrv, metadataCliOperations);
            var metadataCli = Generator.GenerateMetadataCli(metadataCliFull);

            var pathToGenerated = ProgramUtils.GetPathToGenerated();

            metadataCliFull.WritePrettyJson(pathToGenerated, "metadata_cli_full.json");
            metadataCli.WritePrettyJson(pathToGenerated, "metadata_cli.json");

            Console.WriteLine();
            Console.WriteLine("Done! Press any key to continue ...");
            Console.ReadLine();
        }

    }

    public static class ProgramUtils
    {
        public static string GetPathToAppData()
        {
            //var path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase; //.Location
            var path = AppContext.BaseDirectory;
            //path = path.Substring(8);
            // Info credit: https://social.msdn.microsoft.com/Forums/vstudio/en-US/decc53b0-2f53-4aae-b86b-6e786c5f8d90/navigate-up-4-levels-in-directoryfolder-path-to-create-string-reference-to-a-specific-folder?forum=csharpgeneral
            for (int i = 0; i < 4; i++)
            {
                path = Path.GetDirectoryName(path);
            }
            path = Path.Combine(path, "App_Data");
            return path;
        }

        public static string GetPathToGenerated()
        {
            //var path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase; //.Location
            var path = AppContext.BaseDirectory;
            //path = path.Substring(8);
            // Info credit: https://social.msdn.microsoft.com/Forums/vstudio/en-US/decc53b0-2f53-4aae-b86b-6e786c5f8d90/navigate-up-4-levels-in-directoryfolder-path-to-create-string-reference-to-a-specific-folder?forum=csharpgeneral
            for (int i = 0; i < 5; i++)
            {
                path = Path.GetDirectoryName(path);
            }

            path = Path.Combine(path, "_generated");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static T GetJsonFromAppDataAs<T>(this string pathToAppData, string fileName)
        {
            var pathFileName = Path.Combine(pathToAppData, fileName);
            T metadataSrv;
            using (StreamReader r = new StreamReader(pathFileName))
            {
                var jsonText = r.ReadToEnd();
                metadataSrv = JsonConvert.DeserializeObject<T>(jsonText);
            }
            return metadataSrv;
        }

        public static void WritePrettyJson(this object obj, string locationPath, string fileName)
        {
            var path = Path.Combine(locationPath, fileName);
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include });
            File.WriteAllText(path, json);
        }

    }

}
