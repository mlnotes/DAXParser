using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.Common;

namespace DAXParser.CodeParse
{
	class MapData: BaseObjectData
	{
		public static MapData Parse(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				MapData data = new MapData();

				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimStart();
					
					if (line.StartsWith(KeyWords.FIELDS))
					{
						SkipTo(reader, KeyWords.ENDFIELDS);	
					}
					else if (line.StartsWith(KeyWords.MAPPINGS))
					{
						SkipTo(reader, KeyWords.ENDMAPPINGS);
					}
					else if (line.StartsWith(KeyWords.METHODS))
					{
						List<MethodData> methods = ParseMethods(reader);
						foreach (MethodData m in methods)
						{
							data.AddMethod(m);
						}
					}
					else if (line.StartsWith(KeyWords.MAP))
					{
						data.Name = line.Substring(KeyWords.MAP.Length).Trim().Substring(1);
					}
					else if (line.StartsWith(KeyWords.ENDMAP))
					{
						break;
					}
				}

				return data;
			}
		}
	}
}
