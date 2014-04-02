using System.Collections.Generic;
using DAXParser.CodeParse.Common;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.IO;

namespace DAXParser.CodeParse
{
	class TableData:BaseObjectData
	{
		public override string Type
		{
			get { return "TABLE"; }
		}

		private static void ParseReferences(XPOReader reader)
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

		private static void ParseIndices(XPOReader reader)
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
			XPOReader reader;
			TableData data = new TableData();
			using (reader = new XPOReader(path))
			{
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
			}

			data.lineCountOfFile = reader.LineCountOfFile;
			return data;
		}
	}
}
