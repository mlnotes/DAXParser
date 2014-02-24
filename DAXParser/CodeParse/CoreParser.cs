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
		private static string TABLE = "TABLE";
		private static string ENUMTYPE = "ENUMTYPE";
		
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
					else if (line.StartsWith(ENDMETHODS))
					{
						break;
					}
					else if (line.StartsWith(SOURCE))
					{
						MethodData method = ParseMethod(GetName(line, SOURCE), reader);
						clazz.AddMethod(method);
					}
					else
					{
						continue;
					}
				}

				return clazz;
			}
		}

		public static TableData ParseTable(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				TableData table = new TableData();

				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimStart();
					if (line.StartsWith(TABLE))
					{
						table.Name = GetName(line, TABLE);
					}
					else if (line.StartsWith(ENDMETHODS))
					{
						break;
					}
					else if (line.StartsWith(SOURCE))
					{
						MethodData method = ParseMethod(GetName(line, SOURCE), reader);
						table.AddMethod(method);
					}
					else
					{
						continue;
					}
				}

				return table;
			}
		}

		public static EnumData ParseEnum(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				EnumData data = new EnumData();

				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimStart();
					if (line.StartsWith(ENUMTYPE))
					{
						data.Name = GetName(line, ENUMTYPE);
					}
					else if(line.StartsWith("#"))
					{
						data.Lines++;
					}
				}

				return data;
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

		private static string GetName(string line, string type)
		{
			return line.Substring(type.Length).Trim().Substring(1);
		}
	}
}
