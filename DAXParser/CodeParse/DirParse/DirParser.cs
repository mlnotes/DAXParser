using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace DAXParser.CodeParse.DirParse
{
	class ParmObject<T>
		where T: BaseObjectData
	{
		public ParmObject(int count, string path, Func<string, T> func, 
							Dictionary<string, string> prefixOwnership,
							Dictionary<string, string> postfixOwnership,
							Dictionary<string, string> region,
							List<T> objects, ManualResetEvent handle)
		{
			this.Count = count;
			this.Path = path;
			this.ParseFunc = func;
			this.Objects = objects;
			this.PrefixOwnership = prefixOwnership;
			this.PostfixOwnership = postfixOwnership;
			this.Region = region;
			this.Handle = handle;
		}

		public int Count { get; set; }
		public string Path { get; set; }
		public Func<string, T> ParseFunc { get; set; }
		public List<T> Objects { get; set; }
		public Dictionary<string, string> PrefixOwnership { get; set; }
		public Dictionary<string, string> PostfixOwnership { get; set; }
		public Dictionary<string, string> Region { get; set; }
		public ManualResetEvent Handle { get; set; }
	}

	class DirParser
	{

		protected static void AssignOwner<T>(T obj, Dictionary<string, string> prefix, Dictionary<string, string> postfix)
			where T:BaseObjectData
		{
			if (prefix != null && prefix.Count > 0)
			{
				string name = obj.Name.ToUpper();
				string namePre = "";
				for (int i = name.Length; i >= 1; --i)
				{
					namePre = name.Substring(0, i);
					if (prefix.ContainsKey(namePre))
					{
						obj.PrefixOwner = prefix[namePre];
						break;
					}
				}
			}

			if (postfix != null && postfix.Count > 0)
			{
				string name = obj.Name.ToUpper();
				string namePost = "";
				for (int i = 0; i < name.Length; ++i)
				{
					namePost = name.Substring(i);
					if (postfix.ContainsKey(namePost))
					{
						obj.PostfixOwner = postfix[namePost];
						break;
					}
				}
			}
		}

		protected static void AssignRegion<T>(T obj, Dictionary<string, string> region)
			where T:BaseObjectData
		{
			if (region != null && region.Count > 0)
			{
				string name = obj.Name.ToUpper();
				string namePost = "";
				for (int i = 0; i < name.Length; ++i)
				{
					namePost = name.Substring(i);
					if (region.ContainsKey(namePost))
					{
						obj.Region = region[namePost];
						break;
					}
				}
			}
		}

		protected static void ParseFirstLayerFile<T>(ParmObject<T> obj)
			where T:BaseObjectData
		{
			T data = obj.ParseFunc(obj.Path);
			AssignOwner(data, obj.PrefixOwnership, obj.PostfixOwnership);
			AssignRegion(data, obj.Region);
			lock (obj.Objects)
			{
				obj.Objects.Add(data);
				if (obj.Count == obj.Objects.Count)
				{
					obj.Handle.Set();
				}
			}
		}

		protected static void AddObject<T>(List<T> objects, T obj, Dictionary<string, string> prefix, 
						Dictionary<string, string> postfix, Dictionary<string, string> region)
			where T : BaseObjectData
		{
			AssignOwner(obj, prefix, postfix);
			AssignRegion(obj, region);
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

		public static List<T> Parse<T>(string[] layerPaths, string module, Dictionary<string, string> prefix,
										Dictionary<string, string> postfix, Dictionary<string, string> region,
										Func<string, T> parseFunc, string pattern = "*.xpo")
			where T : BaseObjectData
		{
			if (layerPaths == null || layerPaths.Length == 0)
			{
				return new List<T>();
			}

			FileInfo[] files = GetFiles(layerPaths[0], module, pattern);
			List<T> objects = new List<T>();

			// Get all objects in the first layer
			ManualResetEvent[] handles = new ManualResetEvent[]{new ManualResetEvent(false)};
			foreach (FileInfo file in files)
			{
				//T data = parseFunc(file.FullName);
				//AddObject(objects, data, ownership);

				ParmObject<T> parm = new ParmObject<T>(files.Length, file.FullName, parseFunc, 
											prefix, postfix, region, objects, handles[0]);
				ThreadPool.QueueUserWorkItem(obj => ParseFirstLayerFile<T>(obj as ParmObject<T>), parm);
			}

			WaitHandle.WaitAll(handles);

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
				AddObject(objects, data, prefix, postfix, region);
			}

			return objects;
		}
	}
}
