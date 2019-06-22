using System;
using Tools.Models;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using CodeGenerator.Models.Common;

namespace Tools
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
			path = "/" + Path.Combine(path, "App_Data");

			// read json files
			Metadata metadata;
			var pathMetadata = Path.Combine(path, "metadata_mysql.json");
			using (StreamReader r = new StreamReader(pathMetadata))
			{
				var jsonText = r.ReadToEnd();
				metadata = JsonConvert.DeserializeObject<Metadata>(jsonText);
			}

			var pathOperationsDefinition = Path.Combine(path, "operationsDefinition.json");
			OperationsDefinition operationsDefinition;
			using (StreamReader r = new StreamReader(pathOperationsDefinition))
			{
				var jsonText = r.ReadToEnd();
				operationsDefinition = JsonConvert.DeserializeObject<OperationsDefinition>(jsonText);
			}

			// generate code
			var generatedCode = Generator.GenerateModel(metadata, operationsDefinition);

			// save metadata file on disk
			for (int i = 0; i < 3; i++)
			{
				path = Path.GetDirectoryName(path);
			}
			path = "/" + Path.Combine(path, "_generated");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			path = Path.Combine(path, "DataProvider.cs");
			File.WriteAllText(path, generatedCode);
			Console.WriteLine("Done.");
			//Console.ReadLine();
			//Process.Start("notepad.exe", path);
		}
	}
}
