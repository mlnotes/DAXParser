using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse;

namespace DAXParser.CodeParse
{
	class ClassData: BaseObjectData
	{
		public static ClassData Parse(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				ClassData data = new ClassData();

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
				return data;
			}
		}
	}
}
