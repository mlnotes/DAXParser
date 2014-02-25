using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;

namespace DAXParser.CodeParse
{
	class EnumData: BaseObjectData
	{
		public static EnumData Parse(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				EnumData data = new EnumData();

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
						data.Lines++;
					}
				}

				return data;
			}
		}

		public new BaseObjectData MergeWith(BaseObjectData data)
		{
			this.Lines = data.Lines;
			return this;
		}
	}
}
