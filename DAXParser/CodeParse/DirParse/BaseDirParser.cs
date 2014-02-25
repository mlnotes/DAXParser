using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;

namespace DAXParser.CodeParse.DirParse
{
	class BaseDirParser
	{
		protected static void Parse<T>(string[] layerPaths, string module, Func<string, T> parseFunc, string pattern = "*.xpo")
			where T:BaseObjectData
		{
			if (layerPaths == null || layerPaths.Length == 0)
			{
				return;
			}

			FileInfo[] files = GetFiles(layerPaths[0], module, pattern);
			List<T> objects = new List<T>();

			// Get all classes in the first layer
			foreach (FileInfo file in files)
			{
				objects.Add(parseFunc(file.FullName));
			}
			Console.WriteLine("First Layer:{0}", objects.Count);

			// get classes in upper layers
			Dictionary<string, T>[] layers = new Dictionary<string, T>[layerPaths.Length - 1];
			for (int i = 1; i < layerPaths.Length; ++i)
			{
				layers[i - 1] = ParseUpperLayer(layerPaths[i], module, pattern, parseFunc);
				Console.WriteLine("Layer{0}: {1}", i, layers[i - 1].Count);
			}

			// merge classes that exist in the first layer
			for (int i = 0; i < objects.Count; ++i)
			{
				T data = objects[i];
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
				foreach (KeyValuePair<string, T> pair in layers[i])
				{
					objects.Add(pair.Value);
				}
			}

			// TODO dump?
			int methods = 0;
			int lines = 0;
			foreach (T data in objects)
			{
				methods += data.MethodCount;
				lines += data.LineCount;
			}
			Console.WriteLine("Objects:[{0}], Methods:[{1}], Lines:[{2}]", objects.Count, methods, lines);
		}

		protected static FileInfo[] GetFiles(string layerPath, string module, string pattern)
		{
			string path = Path.Combine(layerPath, module);
			DirectoryInfo dir = new DirectoryInfo(path);
			return dir.GetFiles(pattern);
		}

		protected static Dictionary<string, T> ParseUpperLayer<T>(string layerPath, string module, string pattern, Func<string, T> func)
			where T: BaseObjectData
		{
			FileInfo[] files = GetFiles(layerPath, module, pattern);
			Dictionary<string, T> map = new Dictionary<string, T>();
			foreach (FileInfo file in files)
			{
				T data = func(file.FullName);
				map[data.Name.ToUpper()] = data;
			}

			return map;
		}
	}
}
