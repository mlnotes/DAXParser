using System.Collections.Generic;
using System.IO;
using System.Linq;
using DAXParser.CodeParse.Common;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.IO;

namespace DAXParser.CodeParse
{
	abstract class BaseObjectData
	{
		protected Dictionary<string, MethodData> methods = new Dictionary<string, MethodData>();
		protected int lineCount = 0;
		protected int tagCount = 0;
		protected int lineCountOfFile = 0;

		public string Name { get; set; }
		public string PrefixOwner { get; set; }
		public string PostfixOwner { get; set; }
		public string Owner { get; set; }
		public string Region { get; set; }
		public string Country { get; set; }
		public int LineCountOfFile { get { return lineCountOfFile; } }
		public int LineCount { get { return lineCount; } }
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
				lineCountOfFile -= methods[name].LineCount;
			}

			methods[name] = method;
			lineCount += method.LineCount;
			tagCount += method.TagCount;
			lineCountOfFile += method.LineCount;
		}
	
		protected static List<MethodData> ParseMethods(XPOReader reader)
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

		protected static void SkipTo(XPOReader reader, string symbol)
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

		public virtual void Dump(StreamWriter writer, List<string> tags, Dictionary<string, string> tagRegion, string[] tagRegionName)
		{
			Dictionary<string, int> tagMap = GetTagInfo();
			// write basic information

			writer.Write("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", Name, Type, Region, Owner, PrefixOwner, 
				PostfixOwner, Country, LineCount, lineCountOfFile, MethodCount, TagCount);

			if (tagRegionName != null && tagRegionName.Length > 0)
			{
				// get tag region information
				Dictionary<string, int> regionCnt = new Dictionary<string, int>();
				foreach (KeyValuePair<string, int> pair in tagMap)
				{
					for (int i = pair.Key.Length; i > 0; --i)
					{
						string tagPre = pair.Key.Substring(0, i);
						if (tagRegion.ContainsKey(tagPre))
						{
							if(regionCnt.ContainsKey(tagRegion[tagPre]))
							{
								regionCnt[tagRegion[tagPre]] += pair.Value;
							}
							else
							{
								regionCnt[tagRegion[tagPre]] = pair.Value;
							}
							break;
						}
					}
				}

				// write tag region information
				foreach (string region in tagRegionName)
				{
					if (regionCnt.ContainsKey(region))
					{
						writer.Write(",{0}", regionCnt[region]);
					}
					else
					{
						writer.Write(",");
					}
				}
			}

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
