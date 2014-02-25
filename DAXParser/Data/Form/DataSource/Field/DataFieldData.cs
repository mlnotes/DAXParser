using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse;
using System.IO;
using DAXParser.CodeParse.Config;

namespace DAXParser.Data.Form.DataSource.Field
{
	class DataFieldData : BaseObjectData
	{
		public static DataFieldData Parse(string firstLine, StreamReader reader)
		{
			DataFieldData data = new DataFieldData();
			data.Name = firstLine.TrimStart().Substring(KeyWords.DATAFIELD.Length).Trim();

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith(KeyWords.METHODS))
				{
					List<MethodData> methods = CoreParser.ParseMethods(reader);
					foreach (MethodData m in methods)
					{
						data.AddMethod(m);
					}
				}
				else if (line.StartsWith(KeyWords.ENDDATAFIELD))
				{
					break;
				}
			}

			return data;
		}
	}
}
