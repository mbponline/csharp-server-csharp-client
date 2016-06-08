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
            var path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase; //.Location
            path = path.Substring(8);
            for (int i = 0; i < 3; i++)
            {
                path = Path.GetDirectoryName(path);
            }

            // read json files
            Metadata metadata;
            var pathMetadata = Path.Combine(path, "App_Data", "metadata_mysql.json");
            using (StreamReader r = new StreamReader(pathMetadata))
            {
                var jsonText = r.ReadToEnd();
                metadata = JsonConvert.DeserializeObject<Metadata>(jsonText);
            }

            var pathOperationsDefinition = Path.Combine(path, "App_Data", "operationsDefinition.json");
            OperationsDefinition operationsDefinition;
            using (StreamReader r = new StreamReader(pathOperationsDefinition))
            {
                var jsonText = r.ReadToEnd();
                operationsDefinition = JsonConvert.DeserializeObject<OperationsDefinition>(jsonText);
            }

            // generate code
            var generatedCode = Generator.GenerateModel(metadata, operationsDefinition);

            // save metadata file on disk
            for (int i = 0; i < 2; i++)
            {
                path = Path.GetDirectoryName(path);
            }
            path = Path.Combine(path, "_generated");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, "dataProvider.cs");
            File.WriteAllText(path, generatedCode);
            Console.WriteLine("Done. Press a key to exit...");
            Console.ReadLine();
            Process.Start("notepad.exe", path);
        }
    }
}
