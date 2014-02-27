using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.Common;

namespace DAXParser.CodeParse
{
	class TableData:BaseObjectData
	{
		public override string Type
		{
			get { return "TABLE"; }
		}

		private static void ParseReferences(StreamReader reader)
		{
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith(KeyWords.ENDREFERENCES))
				{
					break;
				}
			}
		}

		private static void ParseIndices(StreamReader reader)
		{
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith(KeyWords.ENDINDICES))
				{
					break;
				}
			}
		}

		public static TableData Parse(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				TableData data = new TableData();

				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimStart();
					if (line.StartsWith(KeyWords.TABLE))
					{
						data.Name = line.Substring(KeyWords.TABLE.Length).Trim().Substring(1);
					}
					else if (line.StartsWith(KeyWords.ENDTABLE))
					{
						break;
					}
					else if (line.StartsWith(KeyWords.METHODS))
					{
						List<MethodData> methods = ParseMethods(reader);
						foreach (MethodData m in methods)
						{
							data.AddMethod(m);
						}
					}
					else if (line.StartsWith(KeyWords.REFERENCES))
					{
						ParseReferences(reader);
					}
					else if (line.StartsWith(KeyWords.INDICES))
					{
						ParseIndices(reader);
					}
				}

				return data;
			}
		}
	}
}
