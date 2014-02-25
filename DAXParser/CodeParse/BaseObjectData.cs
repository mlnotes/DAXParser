using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.Common;

namespace DAXParser.CodeParse
{
	class BaseObjectData
	{
		protected Dictionary<string, MethodData> methods = new Dictionary<string, MethodData>();

		public string Name { get; set; }
		public int LineCount { get; set; }
		public virtual int MethodCount { get { return methods.Count; } }
		public Dictionary<string, MethodData> Methods { get { return methods; } }

		public void AddMethod(MethodData method)
		{
			string name = method.Name.ToUpper();
			if (methods.Keys.Contains(name))
			{
				LineCount -= methods[name].LineCount;
			}

			methods[name] = method;
			LineCount += method.LineCount;
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
			Console.WriteLine("BASE MERGE");
			foreach (KeyValuePair<string, MethodData> pair in data.Methods)
			{
				this.AddMethod(pair.Value);
			}
			return this;
		}
	}
}
