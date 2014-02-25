using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse;

namespace DAXParser.CodeParse.Form.DataSource.Field
{
	class ReferenceFieldData: BaseObjectData
	{
		public static ReferenceFieldData Parse(string firstLine, StreamReader reader)
		{
			ReferenceFieldData data = new ReferenceFieldData();
			data.Name = firstLine.TrimStart().Substring(KeyWords.REFERENCEFIELD.Length).Trim();

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith(KeyWords.METHODS))
				{
					List<MethodData> methods = ParseMethods(reader);
					foreach (MethodData m in methods)
					{
						data.AddMethod(m);
					}
				}
				else if (line.StartsWith(KeyWords.ENDREFERENCEFIELD))
				{
					break;
				}
			}

			return data;
		}
	}
}
