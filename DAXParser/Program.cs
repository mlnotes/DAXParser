using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse;
using System.Text.RegularExpressions;
using DAXParser.CodeParse.DirParse;
using DAXParser.CodeParse.Persistent;
using System.IO;


namespace DAXParser
{
	class Program
	{
		static void PatternTest()
		{
			string line = @"if (_assetBook.AssetGroupDepreciation_IN    == NoYes::Yes   ";
			string tags = "(RU|IN|BR|HU|JP|LV|EU|LT|CN|CZ|EE|PL|W)";
			string validChars = "[A-Za-z0-9_]";
			string validEndings = @"([><!=&|\)\s;{]*|[><!=&|\)\s;{]{1,}.*)";
			string pattern = String.Format(@"({0}*)\.({1}*_{2}{3})$", validChars, validChars, tags, validEndings);
			string[] separator = new string[] {"==", "!=", "&&", "||"};

			string[] parts = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < parts.Length; ++i)
			{
				Match match = Regex.Match(parts[i], pattern);
				if (match.Success)
				{
					Console.WriteLine("[Line Match]: {0}", line);
					Console.WriteLine("[Pattern]: {0}", pattern);
					for (int j = 1; j < match.Groups.Count; ++j)
					{
						Console.WriteLine("[Group:{0}]: {1}", j, match.Groups[j].Value);
					}

				}
			}				
		
		}

		static void Main(string[] args)
		{
			long start = DateTime.Now.Ticks;
			Argument arg = Argument.Parse(args);
			if (arg.Dirs.Length == 0)
			{
				Help();
				return;
			}

			ParseCode(arg);
			//PatternTest();
			long end = DateTime.Now.Ticks;
			Console.WriteLine("Total Time in ticks: {0}", end-start);
		}

		static void Help()
		{
			Console.WriteLine("Invalid Arguments");
		}

		static void ParseCode(Argument arg)
		{
			CSVDumper dumper = CSVDumper.GetInstance();
			if (!string.IsNullOrEmpty(arg.Output))
			{
				dumper.Output = arg.Output;
			}

			if(arg.Modules.Length > 0)
			{
				foreach(string module in arg.Modules)
				{
					ParseModule(arg.Dirs, module, arg.Ownership, arg.Pattern, dumper);
				}
			}
			else
			{
				foreach(string module in ModuleDirs.Modules.Keys)
				{
					ParseModule(arg.Dirs, module, arg.Ownership, arg.Pattern, dumper);
				}
			}
			dumper.Dispose();
		}

		static void ParseModule(string[] dirs, String module, Dictionary<string, string> ownership, string pattern, CSVDumper dumper)
		{
			switch (module)
			{
				case ModuleDirs.Name.CLASS:
					Console.WriteLine("[Parsing CLASS ...]");
					List<ClassData> classes = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, ClassData.Parse, pattern);
					dumper.DumpClass(classes);
					break;
				case ModuleDirs.Name.TABLE:
					Console.WriteLine("[Parsing TABLE ...]");
					List<TableData> tables = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, TableData.Parse, pattern);
					dumper.DumpTable(tables);
					break;
				case ModuleDirs.Name.FORM:
					Console.WriteLine("[Parsing FORM ...]");
					List<FormData> forms = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, FormData.Parse, pattern);
					dumper.DumpForm(forms);
					break;
				case ModuleDirs.Name.ENUM:
					Console.WriteLine("[Parsing ENUM ...]");
					List<EnumData> enums = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, EnumData.Parse, pattern);
					dumper.DumpEnum(enums);
					break;
				case ModuleDirs.Name.MAP:
					Console.WriteLine("[Parsing MAP ...]");
					List<MapData> maps = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, MapData.Parse, pattern);
					dumper.DumpMap(maps);
					break;
				case ModuleDirs.Name.QUERY:
					Console.WriteLine("[Parsing QUERY ...]");
					List<QueryData> queries = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, QueryData.Parse, pattern);
					dumper.DumpQuery(queries);
					break;
			}
		}
	}
}
