﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DAXParser
{
    class Argument
    {
		private const string PATTERN = "-PATTERN";
		private const string DIRS = "-DIRS";
		private const string MODULES = "-MODULES";
		private const string OWNERSHIP = "-OWNERSHIP";
		private const string REGION = "-REGION";
		private const string OUTPUT = "-OUTPUT";
		private const string COUNTRY = "-COUNTRY";
		private const string TAGREGION = "-TAGREGION";

		private string pattern = "*.xpo";
		private string output = "dax_parse_result.csv";
        private string[] dirs;
        private string[] modules;
        private Dictionary<string, string> prefixOwnership;
		private Dictionary<string, string> postfixOwnership;
		private Dictionary<string, string> region;
		private Dictionary<string, string> country;
		private Dictionary<string, string> tagRegion;
		
        private Argument()
        {
            dirs = new string[0];
            modules = new string[0];
            prefixOwnership = new Dictionary<string, string>();
			postfixOwnership = new Dictionary<string, string>();
			region = new Dictionary<string, string>();
			country = new Dictionary<string, string>();
			tagRegion = new Dictionary<string, string>();
        }

		public string Pattern { get { return pattern; } }

		public string Output { get { return output; } }

		public string[] Dirs { get { return dirs; } }

		public string[] Modules { get { return modules; } }

		public Dictionary<string, string> PrefixOwnership { get { return prefixOwnership; } }

		public Dictionary<string, string> PostfixOwnership { get { return postfixOwnership; } }

		public Dictionary<string, string> Region { get { return region; } }

		public Dictionary<string, string> Country { get { return country; } }

		public Dictionary<string, string> TagRegion { get { return tagRegion; } }

        public static Argument Parse(string[] args)
        {
            Argument argument = new Argument();
            int i = 0;
            while (i < args.Length)
            {
				args[i] = args[i].ToUpper();
				if (args[i] == PATTERN)
				{
					if (!args[i + 1].StartsWith("-"))
					{
						argument.pattern = args[i + 1];
						i++;
					}
					i++;
				}
				else if (args[i] == OUTPUT)
				{
					if (!args[i + 1].StartsWith("-"))
					{
						argument.output = args[i + 1];
						i++;
					}
					i++;
				}
				else if (args[i] == DIRS)
				{
					List<string> dirs = new List<string>();
					while (++i < args.Length && !args[i].StartsWith("-"))
					{
						dirs.Add(args[i]);
					}
					argument.dirs = dirs.ToArray();
				}
				else if (args[i] == MODULES)
				{
					List<string> modules = new List<string>();
					while (++i < args.Length && !args[i].StartsWith("-"))
					{
						if (ModuleDirs.IsValidModule(args[i]))
						{
							modules.Add(args[i].ToUpper());
						}
					}
					argument.modules = modules.ToArray();
				}
				else if (args[i] == OWNERSHIP)
				{
					while (++i < args.Length && !args[i].StartsWith("-"))
					{
						ParseOwnership(argument.prefixOwnership, argument.postfixOwnership, args[i]);
					}
				}
				else if (args[i] == COUNTRY)
				{
					while (++i < args.Length && !args[i].StartsWith("-"))
					{
						ParseCountry(argument.country, args[i]);
					}

				}
				else if (args[i] == REGION)
				{
					while (++i < args.Length && !args[i].StartsWith("-"))
					{
						ParseRegion(argument.region, args[i]);
					}
				}
				else if (args[i] == TAGREGION)
				{
					while (++i < args.Length && !args[i].StartsWith("-"))
					{
						ParseTagRegion(argument.tagRegion, args[i]);
					}
				}
				else
				{
					++i;
				}
            }
            return argument;
        }

        private static void ParseOwnership(Dictionary<string, string> prefix, Dictionary<string, string> postfix, string path)
        {
			if (!File.Exists(path))
			{
				return;
			}

			using (StreamReader reader = new StreamReader(path))
			{
				string line = reader.ReadLine();
				while (!reader.EndOfStream)
				{
					line = reader.ReadLine().Trim();
					string[] parts = line.Split('\t');
					if (parts.Length >= 2)
					{
						if (parts[1].StartsWith("*"))
						{
							postfix[parts[1].Substring(1).ToUpper()] = parts[0];
						}
						else
						{
							prefix[parts[1].ToUpper()] = parts[0];
						}
					}
				}
			}
        }

		private static void ParseCountry(Dictionary<string, string> country, string path)
		{
			if (!File.Exists(path))
			{
				return;
			}

			using (StreamReader reader = new StreamReader(path))
			{
				string line = reader.ReadLine();
				while (!reader.EndOfStream)
				{
					line = reader.ReadLine().Trim();
					string[] parts = line.Split('\t');
					if (parts.Length >= 2)
					{
						// currently only support postfix
						if (parts[1].StartsWith("*"))
						{
							country[parts[1].Substring(1).ToUpper()] = parts[0];
						}
					}
				}
			}
		}

		private static void ParseTagRegion(Dictionary<string, string> tagRegion, string path)
		{
			if (!File.Exists(path))
			{
				return;
			}

			using (StreamReader reader = new StreamReader(path))
			{
				string line = reader.ReadLine();
				while (!reader.EndOfStream)
				{
					line = reader.ReadLine().Trim();
					string[] parts = line.Split('\t');
					if (parts.Length >= 2)
					{
						// currently only support prefix
						tagRegion[parts[1].ToUpper()] = parts[0];
					}
				}
			}
		}

		private static void ParseRegion(Dictionary<string, string> region, string path)
		{
			if (!File.Exists(path))
			{
				return;
			}

			using (StreamReader reader = new StreamReader(path))
			{
				string line = reader.ReadLine();
				while (!reader.EndOfStream)
				{
					line = reader.ReadLine().Trim();
					string[] parts = line.Split('\t');
					if (parts.Length >= 2)
					{
						// match all
						region[parts[1].ToUpper()] = parts[0];
					}
				}
			}
		}
	}
}

