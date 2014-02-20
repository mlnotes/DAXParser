using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.Data;
using System.Text.RegularExpressions;
using DAXParser.CodeParse.Config;

namespace DAXParser.CodeParse
{
	class CoreParser
	{
		private static string TagPattern = "//.*<(.{1,})>";
		private static string CLASS = "CLASS";
		private static string METHODS = "METHODS";
		private static string SOURCE = "SOURCE";
		private static string ENDMETHODS = "ENDMETHODS";

		public static MethodData ParseMethod(string name, StreamReader reader)
		{
			MethodData method = new MethodData();
			method.Name = name;

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith("#"))
				{
					method.Lines++;
					string tag = GetTag(line);
					if (tag != null)
					{
						method.Tags.Add(new TagData(tag, method.Lines));
					}
				}
				else
				{
					break;
				}
			}
			return method;
		}

		public static ClassData ParseClass(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				ClassData clazz = new ClassData();

				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimStart();
					if (line.StartsWith(CLASS))
					{
						clazz.Name = GetName(line, CLASS);
					}
					if (line.StartsWith(ENDMETHODS))
					{
						break;
					}
					else if (line.StartsWith(SOURCE))
					{
						MethodData method = ParseMethod(GetName(line, SOURCE), reader);
						clazz.AddMethod(method);
						clazz.Lines += method.Lines;
					}
					else
					{
						continue;
					}
				}

				return clazz;
			}
		}

		

		private static string GetTag(string line)
		{
			Match match = Regex.Match(line, CoreParser.TagPattern);
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

		private static string GetName(string line, string obj)
		{
			return line.Substring(obj.Length).Trim().Substring(1);
		}
	}
}
