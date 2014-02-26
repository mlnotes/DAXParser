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
			where T : BaseObjectData
		{
			Parse(layerPaths, module, parseFunc, null, pattern);
		}

		protected static void Parse<T>(string[] layerPaths, string module, Func<string, T> parseFunc, Action<T> dumpFunc, string pattern = "*.xpo")
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

			// merge classes in upper layers
			Dictionary<string, T> upperLayers = new Dictionary<string, T>();
			for (int i = 1; i < layerPaths.Length; ++i)
			{
				files = GetFiles(layerPaths[i], module, pattern);
				foreach (FileInfo file in files)
				{
					T data = parseFunc(file.FullName);
					string key = data.Name.ToUpper();
					if (upperLayers.ContainsKey(key))
					{
						upperLayers[key].MergeWith(data);
					}
					else
					{
						upperLayers[key] = data;
					}
				}
			}

			// merge classes that exist in first layer
			for (int i = 0; i < objects.Count; ++i)
			{
				T data = objects[i];
				string key = data.Name.ToUpper();
				if (upperLayers.ContainsKey(key))
				{
					data.MergeWith(upperLayers[key]);
					upperLayers.Remove(key);
				}
			}

			// mrege classes that do not exist in the first layer
			foreach (T data in upperLayers.Values)
			{
				objects.Add(data);
			}
			

			// TODO dump?
			if (dumpFunc != null)
			{
				foreach (T data in objects)
				{
					dumpFunc(data);
				}
			}
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
