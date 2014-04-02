using System.Collections.Generic;
using DAXParser.CodeParse.Common;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.IO;

namespace DAXParser.CodeParse
{
	class QueryData: BaseObjectData
	{
		public static QueryData Parse(string path)
		{
			XPOReader reader;
			QueryData data = new QueryData();
			using (reader = new XPOReader(path))
			{
				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimStart();
					if (line.StartsWith(KeyWords.QUERY))
					{
						data.Name = line.Substring(KeyWords.QUERY.Length).Trim().Substring(1);
					}
					else if (line.StartsWith(KeyWords.ENDQUERY))
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
					
				}
			}

			data.LineCountOfFile = reader.LineCountOfFile;
			return data;
		}

		public override string Type
		{
			get { return "QUERY"; }
		}
	}
}
