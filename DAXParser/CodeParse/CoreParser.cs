using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.Data;
using System.Text.RegularExpressions;
using DAXParser.CodeParse.Config;
using DAXParser.Data.Form;
using DAXParser.Data.Form.DataSource.Field;

namespace DAXParser.CodeParse
{
	class CoreParser
	{
		private static string TagPattern = "//.*<(.{1,})>";
		private static string CLASS = "CLASS";
		private static string ENDCLASS = "ENDCLASS";
		private static string TABLE = "TABLE";
		private static string ENDTABLE = "ENDTABLE";
		private static string ENUMTYPE = "ENUMTYPE";
		private static string ENDENUMTYPE = "ENDENUMTYPE";

		private static string FORM = "FORM";
		private static string ENDFORM = "ENDFORM";
		

		
		private static string SOURCE = "SOURCE";
		private static string ENDMETHODS = "ENDMETHODS";

		private static MethodData ParseMethod(string name, StreamReader reader)
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

		public static List<MethodData> ParseMethods(StreamReader reader)
		{
			List<MethodData> methods = new List<MethodData>();

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith(SOURCE))
				{
					methods.Add(ParseMethod(GetName(line, SOURCE), reader));
				}
				else if (line.StartsWith(ENDMETHODS))
				{
					break;
				}
				else
				{
					continue;
				}

			}

			return methods;

		}

		public static List<DataFieldData> ParseDataFields(StreamReader reader)
		{
			// TODO
			return null;
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
					else if (line.StartsWith(ENDCLASS))
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
					else if (line.StartsWith(ENDTABLE))
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
					else if (line.StartsWith(ENDENUMTYPE))
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

		public static FormData ParseForm(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				FormData data = new FormData();

				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimStart();
					if (line.StartsWith(FORM))
					{
						data.Name = GetName(line, FORM);
					}
					else if (line.StartsWith(ENDFORM))
					{
						break;
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

			// TODO


			return null;
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

		public static string GetName(string line, string type)
		{
			return line.Substring(type.Length).Trim().Substring(1);
		}
	}
}
