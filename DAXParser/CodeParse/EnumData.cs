using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.IO;

namespace DAXParser.CodeParse
{
	class EnumData: BaseObjectData
	{
		public static EnumData Parse(string path)
		{
			XPOReader reader;
			EnumData data = new EnumData();
			using (reader = new XPOReader(path))
			{
				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimStart();
					if (line.StartsWith(KeyWords.ENUMTYPE))
					{
						data.Name = line.Substring(KeyWords.ENUMTYPE.Length).Trim().Substring(1);
					}
					else if (line.StartsWith(KeyWords.ENDENUMTYPE))
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
			this.lineCount = data.LineCount;
			return this;
		}

		public override string Type
		{
			get { return "ENUM"; }
		}
	}
}
