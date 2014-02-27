using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DAXParser.CodeParse.DirParse
{
	class DirParser
	{
		protected static void AddObject<T>(List<T> objects, T obj, Dictionary<string, string> ownership)
			where T : BaseObjectData
		{
			if (ownership != null)
			{
				string name = obj.Name.ToUpper();
				string prefix = "";
				for (int i = name.Length; i >= 1; --i)
				{
					prefix = name.Substring(0, i);
					if (ownership.ContainsKey(prefix))
					{
						obj.Owner = ownership[prefix];
						break;
					}
				}
			}

			objects.Add(obj);
		}

		protected static FileInfo[] GetFiles(string layerPath, string module, string pattern)
		{
			string path = Path.Combine(layerPath, module);
			DirectoryInfo dir = new DirectoryInfo(path);
			return dir.GetFiles(pattern);
		}

		protected static Dictionary<string, T> ParseUpperLayer<T>(string layerPath, string module, string pattern, Func<string, T> func)
			where T : BaseObjectData
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

		public static List<T> Parse<T>(string[] layerPaths, string module, Dictionary<string, string> ownership, Func<string, T> parseFunc, string pattern = "*.xpo")
			where T : BaseObjectData
		{
			if (layerPaths == null || layerPaths.Length == 0)
			{
				return new List<T>();
			}

			FileInfo[] files = GetFiles(layerPaths[0], module, pattern);
			List<T> objects = new List<T>();

			// Get all objects in the first layer
			foreach (FileInfo file in files)
			{
				T data = parseFunc(file.FullName);
				AddObject(objects, data, ownership);
			}

			// merge objects in upper layers
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

			// merge objects that exist in first layer
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

			// mrege objects that do not exist in the first layer
			foreach (T data in upperLayers.Values)
			{
				AddObject(objects, data, ownership);
			}

			return objects;
		}
	}
}
