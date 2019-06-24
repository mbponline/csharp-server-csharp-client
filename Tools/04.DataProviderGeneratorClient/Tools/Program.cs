using Newtonsoft.Json;
using System;
using System.IO;
using Tools.Modules;
using MetadataCli = Tools.Modules.Common.MetadataCli;

namespace Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            var pathToAppData = ProgramUtils.GetPathToAppData();
            var metadataCliFull = pathToAppData.GetJsonFromAppDataAs<MetadataCli.Metadata>("metadata_cli_full.json");

            // generate code
            var generatedCode = Generator.Generate(metadataCliFull);

            // save generated code on file to disk
            var pathToGenerated = ProgramUtils.GetPathToGenerated();
            generatedCode.WriteToFile(pathToGenerated, "DataProvider.cs");

            Console.WriteLine();
            Console.WriteLine("Done! Press a key to exit...");
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

        public static void WriteToFile(this string source, params string[] paths)
        {
            var path = Path.Combine(paths);
            var folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            File.WriteAllText(path, source);
        }

    }

}
