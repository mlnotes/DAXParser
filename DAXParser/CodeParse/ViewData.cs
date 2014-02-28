using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.Common;

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
			using (StreamReader reader = new StreamReader(path))
			{
				ViewData data = new ViewData();

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
				return data;
			}
		}
	}
}
