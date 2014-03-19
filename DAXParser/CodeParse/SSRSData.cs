using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;

namespace DAXParser.CodeParse
{
	class SSRSData: BaseObjectData
	{

		public override string Type
		{
			get { return "SSRS"; }
		}

		public static SSRSData Parse(string path)
		{
			SSRSData data = new SSRSData();
			using (StreamReader reader = new StreamReader(path))
			{
				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimStart();
					if (line.StartsWith(KeyWords.REPORT))
					{
						data.Name = line.Substring(KeyWords.REPORT.Length).Trim().Substring(1);
					}
					else if (line.StartsWith(KeyWords.ENDREPORT))
					{
						break;
					}
					else if (line.StartsWith("#"))
					{
						data.lineCount++;
					}
				}
			}
			return data;
		}

		public override BaseObjectData MergeWith(BaseObjectData data)
		{
			SSRSData ssrsData = data as SSRSData;
			this.lineCount = ssrsData.lineCount;
			return this;
		}
	}
}
