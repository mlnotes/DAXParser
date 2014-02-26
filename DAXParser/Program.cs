using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse;
using System.Text.RegularExpressions;
using DAXParser.CodeParse.DirParse;

namespace DAXParser
{
	class Program
	{
		static void Main(string[] args)
		{
			Argument arg = Argument.Parse(args);
			if (arg.Dirs.Length == 0)
			{
				Help();
				return;
			}

			ParseCode(arg);

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
					ClassDirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), pattern);
					break;
				case ModuleDirs.Name.TABLE:
					TableDirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), pattern);
					break;
				case ModuleDirs.Name.FORM:
					FormDirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), pattern);
					break;
				case ModuleDirs.Name.ENUM:
					EnumDirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), pattern);
					break;
				case ModuleDirs.Name.MAP:
					MapDirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), pattern);
					break;
				case ModuleDirs.Name.QUERY:
					QueryDirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), pattern);
					break;
			}
		}
	}
}
