using System.Collections.Generic;
using DAXParser.CodeParse.Common;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.IO;

namespace DAXParser.CodeParse
{
	class ClassData: BaseObjectData
	{
		public override string Type
		{
			get { return "CLASS"; }
		}

		public static ClassData Parse(string path)
		{
			ClassData data = new ClassData();
			XPOReader reader;
			using (reader = new XPOReader(path))
			{
				
				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimStart();
					if (line.StartsWith(KeyWords.CLASS))
					{
						data.Name = line.Substring(KeyWords.CLASS.Length).Trim().Substring(1);
					}
					else if(line.StartsWith(KeyWords.INTERFACE))
					{
						data.Name = line.Substring(KeyWords.INTERFACE.Length).Trim().Substring(1);
					}
					else if (line.StartsWith(KeyWords.ENDCLASS))
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
	}
}
