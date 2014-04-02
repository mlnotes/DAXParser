using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse;
using DAXParser.CodeParse.Common;
using DAXParser.CodeParse.IO;

namespace DAXParser.CodeParse.Form.Design
{
	class ControlData: BaseObjectData
	{
		public override string Type
		{
			get { return "CONTROL"; }
		}

		public static ControlData Parse(string firstLine, XPOReader reader)
		{
			ControlData data = new ControlData();

			int pos = firstLine.LastIndexOf('#');
			if (pos >= 0)
			{
				data.Name = firstLine.Substring(pos + 1).Trim();
			}

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
