using System.Collections.Generic;
using DAXParser.CodeParse.Common;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.IO;

namespace DAXParser.CodeParse
{
	class ViewData:BaseObjectData
	{
		public override string Type
		{
			get { return "VIEW"; }
		}

		public static ViewData Parse(string path)
		{
			ViewData data = new ViewData();
			XPOReader reader;
			using (reader = new XPOReader(path))
			{
				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().Trim();
					if (line.StartsWith(KeyWords.VIEW) && string.IsNullOrEmpty(data.Name))
					{
						data.Name = line.Substring(KeyWords.VIEW.Length).Trim().Substring(1);
					}
					else if (line == KeyWords.ENDVIEW)
					{
						break;
					}
					else if (line == KeyWords.METHODS)
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
