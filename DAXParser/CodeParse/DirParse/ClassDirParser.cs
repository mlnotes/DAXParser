using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;

namespace DAXParser.CodeParse.DirParse
{
	class ClassDirParser
	{
		public static void Parse(string[] layerPaths, string pattern = "*.xpo")
		{
			if (layerPaths == null || layerPaths.Length == 0)
			{
				return;
			}

			FileInfo[] files = GetFiles(layerPaths[0], pattern);
			List<ClassData> classes = new List<ClassData>();

			// Get all classes in the first layer
			foreach (FileInfo file in files)
			{
				classes.Add(ClassData.Parse(file.FullName));
			}
			Console.WriteLine("First Layer:{0}", classes.Count);

			// get classes in upper layers
			Dictionary<string, ClassData>[] layers = new Dictionary<string,ClassData>[layerPaths.Length-1];
			for (int i = 1; i < layerPaths.Length; ++i)
			{
				layers[i - 1] = ParseUpperLayer(layerPaths[i], pattern);
				Console.WriteLine("Layer{0}: {1}",i, layers[i-1].Count);
			}

			// merge classes that exist in the first layer
			for (int i = 0; i < classes.Count; ++i)
			{
				ClassData data = classes[i];
				string name = data.Name.ToUpper();
				for (int j = 0; j < layers.Length; ++j)
				{
					if (layers[j].ContainsKey(name))
					{
						data.MergeWith(layers[j][name]);
						layers[j].Remove(name);
					}
				}
			}

			// mrege classes that do not exist in the first layer
			for (int i = 0; i < layers.Length; ++i)
			{
				foreach (KeyValuePair<string, ClassData> pair in layers[i])
				{
					classes.Add(pair.Value);
				}
			}

			// TODO dump?
			int methods = 0;
			int lines = 0;
			foreach (ClassData data in classes)
			{
				methods += data.Methods.Count;
				lines += data.Lines;
			}
			Console.WriteLine("Objects:[{0}], Methods:[{1}], Lines:[{2}]", classes.Count, methods, lines);
		}

		private static FileInfo[] GetFiles(string layerPath, string pattern)
		{
			string path = Path.Combine(layerPath, ModuleDirs.CLASS);
			DirectoryInfo dir = new DirectoryInfo(path);
			return dir.GetFiles(pattern);
		}

		private static Dictionary<string, ClassData> ParseUpperLayer(string layerPath, string pattern)
		{
			FileInfo[] files = GetFiles(layerPath, pattern);
			Dictionary<string, ClassData> classMap = new Dictionary<string, ClassData>();
			foreach (FileInfo file in files)
			{
				ClassData data = ClassData.Parse(file.FullName);
				classMap[data.Name.ToUpper()] = data;
			}

			return classMap;
		}
	}
}
