using Tools.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            var metadata = MetadataGenerator.Generate();

            var path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = path.Substring(8);
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

            path = Path.Combine(path, "metadata_mysql.json");

            var json = JsonConvert.SerializeObject(metadata, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, DefaultValueHandling = DefaultValueHandling.Include });

            Console.WriteLine(json);

            File.WriteAllText(path, json);

            Console.WriteLine();
            Console.WriteLine("Press any key to continue ...");

            Console.ReadLine();

            Process.Start("notepad.exe", path);
        }
    }

}
