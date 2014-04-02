using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace DAXParser.CodeParse.DirParse
{
	class IntRef
	{
		public int Value { get; set; }

		public IntRef(int val = 0)
		{
			this.Value = val;
		}
	}

	class ParmObject<T>
		where T : BaseObjectData
	{
		public ParmObject(int count, IntRef processed, string path, Func<string, T> func,
							Dictionary<string, string> prefixOwnership,
							Dictionary<string, string> postfixOwnership,
							Dictionary<string, string> country,
							Dictionary<string, string> region,
							Dictionary<string, T> objects,
							ManualResetEvent handle)
		{
			this.Count = count;
			this.Processed = processed;
			this.Path = path;
			this.ParseFunc = func;
			this.Objects = objects;
			this.PrefixOwnership = prefixOwnership;
			this.PostfixOwnership = postfixOwnership;
			this.Country = country;
			this.Region = region;
			this.Handle = handle;
		}

		public int Count { get; set; }
		public IntRef Processed { get; set; }
		public string Path { get; set; }
		public Func<string, T> ParseFunc { get; set; }
		public Dictionary<string, T> Objects { get; set; }
		public Dictionary<string, string> PrefixOwnership { get; set; }
		public Dictionary<string, string> PostfixOwnership { get; set; }
		public Dictionary<string, string> Country { get; set; }
		public Dictionary<string, string> Region { get; set; }
		public ManualResetEvent Handle { get; set; }
	}

	class DirParser
	{

		protected static void AssignOwnerAndRegion<T>(T obj, Dictionary<string, string> prefix,
								Dictionary<string, string> postfix, Dictionary<string, string> region)
			where T : BaseObjectData
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

			obj.Owner = string.IsNullOrEmpty(obj.PostfixOwner) ? obj.PrefixOwner : obj.PostfixOwner;
			if (region != null && region.Count > 0 && !string.IsNullOrEmpty(obj.Owner))
			{
				if (region.ContainsKey(obj.Owner.ToUpper()))
				{
					obj.Region = region[obj.Owner.ToUpper()];
				}
			}
		}

		protected static void AssignCountry<T>(T obj, Dictionary<string, string> country)
			where T : BaseObjectData
		{
			if (country != null && country.Count > 0)
			{
				string name = obj.Name.ToUpper();
				string namePost = "";
				for (int i = 0; i < name.Length; ++i)
				{
					namePost = name.Substring(i);
					if (country.ContainsKey(namePost))
					{
						obj.Country = country[namePost];
						break;
					}
				}
			}
		}


		protected static void ParseLayerFile<T>(ParmObject<T> obj)
			where T : BaseObjectData
		{
			T data = obj.ParseFunc(obj.Path);
			AssignOwnerAndRegion(data, obj.PrefixOwnership, obj.PostfixOwnership, obj.Region);
			AssignCountry(data, obj.Country);

			// only lock on the same object can prevent concurrency
			// all ParmObject instances share the same objects
			lock (obj.Objects)
			{
				string key = data.Name.ToUpper();
				if (obj.Objects.ContainsKey(key))
				{
					obj.Objects[key].MergeWith(data);
				}
				else
				{
					obj.Objects[key] = data;
				}

				obj.Processed.Value++;
				if (obj.Processed.Value >= obj.Count)
				{
					obj.Handle.Set();
				}
			}
		}

		protected static FileInfo[] GetFiles(string layerPath, string module, string pattern)
		{
			string path = Path.Combine(layerPath, module);
			DirectoryInfo dir = new DirectoryInfo(path);

			// in case of invalid dir
			try
			{
				return dir.GetFiles(pattern);
			}
			catch (Exception e)
			{
				return new FileInfo[0];
			}
		}

		public static List<T> Parse<T>(string[] layerPaths, string module, Dictionary<string, string> prefix,
										Dictionary<string, string> postfix, Dictionary<string, string> country,
										Dictionary<string, string> region, Func<string, T> parseFunc,
										string pattern = "*.xpo")
			where T : BaseObjectData
		{
			if (layerPaths == null || layerPaths.Length == 0)
			{
				return new List<T>();
			}

			Dictionary<string, T> objects = new Dictionary<string, T>();
			IntRef processed = new IntRef(0);
			ManualResetEvent[] handles = new ManualResetEvent[] { new ManualResetEvent(false) };
			foreach (string layerPath in layerPaths)
			{
				FileInfo[] files = GetFiles(layerPath, module, pattern);
				processed.Value = 0;
				handles[0].Reset();
				foreach (FileInfo file in files)
				{
					ParmObject<T> parm = new ParmObject<T>(files.Length, processed, file.FullName, parseFunc,
											prefix, postfix, country, region, objects, handles[0]);
					ThreadPool.QueueUserWorkItem(obj => ParseLayerFile<T>(obj as ParmObject<T>), parm);
				}
				WaitHandle.WaitAll(handles);
			}

			return objects.Values.ToList();
		}
	}
}
