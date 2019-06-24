using Newtonsoft.Json;
using System;
using System.IO;
using Tools.Modules;

namespace Tools
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var metadataSrv = Generator.Generate();

            var pathToGenerated = ProgramUtils.GetPathToGenerated();

            metadataSrv.WritePrettyJson(pathToGenerated, "metadata_srv.json");

            Console.WriteLine();
            Console.WriteLine("Done! Press any key to continue ...");
            Console.ReadLine();
        }
    }

    public static class ProgramUtils
    {

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

        public static void WritePrettyJson(this object obj, string locationPath, string fileName)
        {
            var path = Path.Combine(locationPath, fileName);
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include });
            File.WriteAllText(path, json);
        }

    }

}
