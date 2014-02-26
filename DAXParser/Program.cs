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
			string line = @"#    boolean             depreciationParameters = AssetParameters::checkAssetParameters_IN();";
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
			Argument arg = Argument.Parse(args);
			if (arg.Dirs.Length == 0)
			{
				Help();
				return;
			}

			//ParseCode(arg);
			PatternTest();
		}

		static void Help()
		{
			Console.WriteLine("Invalid Arguments");
		}

		static void ParseCode(Argument arg)
		{
			if(arg.Modules.Length > 0)
			{
				foreach(string module in arg.Modules)
				{
					ParseModule(arg.Dirs, module, arg.Pattern);
				}
			}
			else
			{
				foreach(string module in ModuleDirs.Modules.Keys)
				{
					ParseModule(arg.Dirs, module, arg.Pattern);
				}
			}
		}

		static void ParseModule(string[] dirs, String module, string pattern)
		{
			switch (module)
			{
				case ModuleDirs.Name.CLASS:
					Console.WriteLine("[CLASS]");
					ClassDirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), pattern);
					break;
				case ModuleDirs.Name.TABLE:
					Console.WriteLine("[TABLE]");
					TableDirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), pattern);
					break;
				case ModuleDirs.Name.FORM:
					Console.WriteLine("[FORM]");
					FormDirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), pattern);
					break;
				case ModuleDirs.Name.ENUM:
					Console.WriteLine("[ENUM]");
					EnumDirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), pattern);
					break;
				case ModuleDirs.Name.MAP:
					Console.WriteLine("[MAP]");
					MapDirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), pattern);
					break;
				case ModuleDirs.Name.QUERY:
					Console.WriteLine("[QUERY]");
					QueryDirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), pattern);
					break;
			}
		}
	}
}
