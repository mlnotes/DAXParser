using System.Collections.Generic;
using DAXParser.CodeParse.Common;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.IO;

namespace DAXParser.CodeParse
{
	class MapData: BaseObjectData
	{
		public override string Type
		{
			get { return "MAP"; }
		}

		public static MapData Parse(string path)
		{
			XPOReader reader;
			MapData data = new MapData();
			using (reader = new XPOReader(path))
			{
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
			}

			data.LineCountOfFile = reader.LineCountOfFile;
			return data;
		}
	}
}
