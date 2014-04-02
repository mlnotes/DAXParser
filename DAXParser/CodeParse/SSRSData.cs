using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.IO;

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
			XPOReader reader;
			using (reader = new XPOReader(path))
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

			data.LineCountOfFile = reader.LineCountOfFile;
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
