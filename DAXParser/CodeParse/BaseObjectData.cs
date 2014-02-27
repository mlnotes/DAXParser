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
		public string Owner { get; set; }
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
	}
}
