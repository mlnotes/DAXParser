using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.Common;

namespace DAXParser.CodeParse
{
	abstract class BaseObjectData
	{
		protected Dictionary<string, MethodData> methods = new Dictionary<string, MethodData>();
		protected int lineCount = 0;
		protected int tagCount = 0;

		public string Name { get; set; }
		public string PrefixOwner { get; set; }
		public string PostfixOwner { get; set; }
		public string Country { get; set; }
		public virtual int LineCount { get { return lineCount; } }
		public virtual int TagCount { get { return tagCount; } }
		public virtual int MethodCount { get { return methods.Count; } }
		public abstract string Type { get;}
		public Dictionary<string, MethodData> Methods { get { return methods; } }

		public virtual void AddMethod(MethodData method)
		{
			string name = method.Name.ToUpper();
			if (methods.Keys.Contains(name))
			{
				lineCount -= methods[name].LineCount;
				tagCount -= methods[name].TagCount;
			}

			methods[name] = method;
			lineCount += method.LineCount;
			tagCount += method.TagCount;
		}
	
		protected static List<MethodData> ParseMethods(StreamReader reader)
		{
			List<MethodData> methods = new List<MethodData>();

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith(KeyWords.SOURCE))
				{
					methods.Add(MethodData.Parse(line, reader));
				}
				else if (line.StartsWith(KeyWords.ENDMETHODS))
				{
					break;
				}
				else
				{
					continue;
				}

			}

			return methods;

		}

		protected static void SkipTo(StreamReader reader, string symbol)
		{
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith(symbol))
				{
					break;
				}
			}
		}

		public virtual BaseObjectData MergeWith(BaseObjectData data)
		{
			foreach (MethodData method in data.Methods.Values)
			{
				this.AddMethod(method);
			}
			return this;
		}

		protected virtual Dictionary<string, int> GetTagInfo()
		{
			Dictionary<string, int> tagMap = new Dictionary<string, int>();
			foreach (MethodData m in methods.Values)
			{
				foreach (TagData t in m.Tags)
				{
					if (tagMap.ContainsKey(t.Name))
					{
						tagMap[t.Name] += t.LineCount;
					}
					else
					{
						tagMap[t.Name] = t.LineCount;
					}
				}
			}

			return tagMap;
		}

		public virtual void Dump(StreamWriter writer, List<string> tags)
		{
			Dictionary<string, int> tagMap = GetTagInfo();
			// write basic information
			string owner = string.IsNullOrEmpty(PostfixOwner) ? PrefixOwner : PostfixOwner;
			writer.Write("{0},{1},{2},{3},{4},{5},{6},{7},{8}", Name, Type, owner, PrefixOwner, 
				PostfixOwner, Country, LineCount, MethodCount, TagCount);


			// write tag information
			foreach (string tag in tags)
			{
				if (tagMap.ContainsKey(tag))
				{
					writer.Write(",{0}", tagMap[tag]);
					tagMap.Remove(tag);
				}
				else
				{
					writer.Write(",");
				}
			}

			// append new tag
			foreach (KeyValuePair<string, int> pair in tagMap)
			{
				writer.Write(",{0}", pair.Value);
				tags.Add(pair.Key);
			}

			writer.WriteLine();
		}
	}
}
