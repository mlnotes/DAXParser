using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.Common;

namespace DAXParser.CodeParse
{
	class QueryData: BaseObjectData
	{
		public static QueryData Parse(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				QueryData data = new QueryData();

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

				return data;
			}
		}
	}
}
