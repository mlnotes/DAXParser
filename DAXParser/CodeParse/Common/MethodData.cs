using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;
using System.Text.RegularExpressions;

namespace DAXParser.CodeParse.Common
{
	class MethodData
	{
		private List<TagData> tags = new List<TagData>();

		public string Name { get; set; }
		public int LineCount { get; set; }
		public List<TagData> Tags { get { return tags; } }

		private static string GetTag(string line)
		{
			Match match = Regex.Match(line, CountryTags.TagPattern);
			if (match.Success)
			{
				string tag = match.Groups[1].Value;
				if (tag.Contains("/") && CountryTags.IsCountryTag(tag.Substring(1)))
				{
					return tag;
				}
				else if (CountryTags.IsCountryTag(tag))
				{
					return tag;
				}
			}

			return null;
		}

		public static MethodData Parse(string firstLine, StreamReader reader)
		{
			MethodData method = new MethodData();
			method.Name = firstLine.Substring(KeyWords.SOURCE.Length).Trim().Substring(1);

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith("#"))
				{
					method.LineCount++;
					string tag = GetTag(line);
					if (tag != null)
					{
						method.Tags.Add(new TagData(tag, method.LineCount));
					}
				}
				else
				{
					break;
				}
			}
			return method;
		}
	}
}
