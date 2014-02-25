using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse;

namespace DAXParser.Data.Form.Design
{
	class ControlData: BaseObjectData
	{
		public static ControlData Parse(StreamReader reader)
		{
			ControlData data = new ControlData();

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith(KeyWords.ENDCONTROL))
				{
					break;
				}
				else if (line.StartsWith(KeyWords.Name))
				{
					data.Name = line.Substring(KeyWords.Name.Length).Trim().Substring(1);
				}
				else if (line.StartsWith(KeyWords.METHODS))
				{
					List<MethodData> methods = CoreParser.ParseMethods(reader);
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
