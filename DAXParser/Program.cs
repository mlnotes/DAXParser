using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse;
using System.Text.RegularExpressions;
using DAXParser.CodeParse.DirParse;
using DAXParser.CodeParse.Persistent;
using System.IO;
using System.Threading;


namespace DAXParser
{
	class Program
	{
		static void Main(string[] args)
		{
			long start = DateTime.Now.Ticks;
			Argument arg = Argument.Parse(args);
			if (arg.Dirs.Length == 0)
			{
				Help();
				return;
			}

			Program program = new Program();
			program.ParseCode(arg);
			long end = DateTime.Now.Ticks;
			Console.WriteLine("Total Time in ticks: {0}", end - start);
		}

		static void Help()
		{
			string usage =
				"Usage: DAXParser [-modules module-list] [-output output]\n" +
				"                 [-pattern pattern] [-ownership ownership-list]\n" +
				"                 -dirs dir-list";

			string options =
				"Options:\n" +
				"-modules module-list       One or some of class,form,table,map,query,enum,\n" +
				"                           separated by space, if not used, all modules will\n" +
				"                           be parsed.\n" +
				"-output output             The file you want to write the result.\n" +
				"-pattern pattern           Choose files you want to parse, such as *.xpo.\n" +
				"-ownership ownership-list  Files used to set the ownership of objects, if \n" +
				"                           conflicts, the latter one works.\n" +
				"-dirs dir-list             Directories of dax layers, the lower layer should \n" +
				"                           appear former.";

			string help = string.Format("{0}\n\n\n{1}", usage, options);

			Console.WriteLine(help);
		}

		public void ParseCode(Argument arg)
		{
			CSVDumper dumper = CSVDumper.GetInstance();
			
			if (!string.IsNullOrEmpty(arg.Output))
			{
				dumper.Output = arg.Output;
			}

			if (arg.Modules.Length > 0)
			{
				foreach (string module in arg.Modules)
				{
					ParseModule(arg.Dirs, module, arg.Ownership, arg.Pattern, dumper);
				}
			}
			else
			{
				foreach (string module in ModuleDirs.Modules.Keys)
				{
					ParseModule(arg.Dirs, module, arg.Ownership, arg.Pattern, dumper);
				}
			}

			dumper.Dispose();
		}

		public void ParseModule(string[] dirs, String module, Dictionary<string, string> ownership, string pattern, CSVDumper dumper)
		{
			switch (module)
			{
				case ModuleDirs.Name.CLASS:
					Console.WriteLine("[Parsing CLASS ...]");
					List<ClassData> classes = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, ClassData.Parse, pattern);
					dumper.Dump(classes);
					break;
				case ModuleDirs.Name.TABLE:
					Console.WriteLine("[Parsing TABLE ...]");
					List<TableData> tables = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, TableData.Parse, pattern);
					dumper.Dump(tables);
					break;
				case ModuleDirs.Name.FORM:
					Console.WriteLine("[Parsing FORM ...]");
					List<FormData> forms = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, FormData.Parse, pattern);
					dumper.Dump(forms);
					break;
				case ModuleDirs.Name.ENUM:
					Console.WriteLine("[Parsing ENUM ...]");
					List<EnumData> enums = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, EnumData.Parse, pattern);
					dumper.Dump(enums);
					break;
				case ModuleDirs.Name.MAP:
					Console.WriteLine("[Parsing MAP ...]");
					List<MapData> maps = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, MapData.Parse, pattern);
					dumper.Dump(maps);
					break;
				case ModuleDirs.Name.QUERY:
					Console.WriteLine("[Parsing QUERY ...]");
					List<QueryData> queries = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, QueryData.Parse, pattern);
					dumper.Dump(queries);
					break;
				case ModuleDirs.Name.VIEW:
					Console.WriteLine("[Parsing VIEW ...]");
					List<ViewData> views = DirParser.Parse(dirs, ModuleDirs.GetModuleDir(module), ownership, ViewData.Parse, pattern);
					dumper.Dump(views);
					break;
			}
		}
	}
}
