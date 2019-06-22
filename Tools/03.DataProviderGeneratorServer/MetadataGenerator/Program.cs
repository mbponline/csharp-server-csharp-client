using System;
using MetadataGenerator.Models;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using CodeGenerator.Models.Common;

namespace MetadataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = path.Substring(8);
            for (int i = 0; i < 3; i++)
            {
                path = Path.GetDirectoryName(path);
            }
            path = Path.Combine(path, "App_Data", "metadata_mysql.json");

            // read json file
            Metadata metadata;
            using (StreamReader r = new StreamReader(path))
            {
                var jsonText = r.ReadToEnd();
                metadata = JsonConvert.DeserializeObject<Metadata>(jsonText);
            }

            // generate metadata
            var dataProvider = Generator.GenerateModel(metadata);

            // save metadata file on disk
            for (int i = 0; i < 4; i++)
            {
                path = Path.GetDirectoryName(path);
            }
            path = Path.Combine(path, "_generated");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, "dataProvider.cs");
            File.WriteAllText(path, dataProvider);
            Console.WriteLine("Done. Press a key to exit...");
            Console.ReadLine();
            Process.Start("notepad.exe", path);
        }
    }
}
