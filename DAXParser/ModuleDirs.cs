using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAXParser
{
	class ModuleDirs
	{
		public class Name
		{
			public const string CLASS = "CLASS";
			public const string TABLE = "TABLE";
			public const string FORM = "FORM";
			public const string QUERY = "QUERY";
			public const string MAP = "MAP";
			public const string ENUM = "ENUM";
		}

		public class Dir
		{
			public const string CLASS = "Classes";
			public const string FORM = "Forms";
			public const string QUERY = "Queries";

			public const string TABLE = @"Data Dictionary\Tables";
			public const string MAP = @"Data Dictionary\Maps";
			public const string ENUM = @"Data Dictionary\Base Enums";
		}

		

		private static Dictionary<string, string> modules = new Dictionary<string, string>() { 
			{Name.CLASS, Dir.CLASS},
			{Name.FORM, Dir.FORM},
			{Name.QUERY, Dir.QUERY},
			{Name.TABLE, Dir.TABLE},
			{Name.MAP, Dir.MAP},
			{Name.ENUM, Dir.ENUM}
		};

		public static Dictionary<string, string> Modules { get { return modules; } }

		public static string GetModuleDir(string module)
		{
			return modules[module.ToUpper()];
		}

		public static bool IsValidModule(string module)
		{
			return modules.ContainsKey(module.ToUpper());
		}
	}
}