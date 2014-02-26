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

		private string pattern = "*.xpo";
        private string[] dirs;
        private string[] modules;
        private Dictionary<string, string> ownership;
        
        private Argument()
        {
            dirs = new string[0];
            modules = new string[0];
            ownership = new Dictionary<string, string>();
        }

		public string Pattern
		{
			get { return pattern; }
		}

        public string[] Dirs
        {
            get { return dirs; }
        }

        public string[] Modules
        {
            get { return modules; }
        }

        public Dictionary<string, string> Ownership
        {
            get { return ownership; }
        }

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
                else if (args[i] == DIRS)
                {
					List<string> dirs = new List<string>();
                    while (++i < args.Length && !args[i].StartsWith("-"))
                    {
                        dirs.Add(args[i]);
                    }
					argument.dirs = dirs.ToArray();
                }
                else if(args[i] == MODULES)
                {
					List<string> modules = new List<string>();
                    while (++i < args.Length && !args[i].StartsWith("-"))
                    {
						if(ModuleDirs.IsValidModule(args[i]))
						{
							modules.Add(args[i].ToUpper());
						}
                    }
					argument.modules = modules.ToArray();
                }
                else if(args[i] == OWNERSHIP)
                {
                    while (++i < args.Length && !args[i].StartsWith("-"))
                    {
                        parseOwnership(argument.ownership, args[i]);
                    }
                }
                else
                {
                    ++i;
                }
            }
            return argument;
        }

        private static void parseOwnership(Dictionary<string, string> dict, string path)
        {
            string[] lines = File.ReadAllLines(path);

            for (int i = 1; i < lines.Length; ++i)
            {
                string[] parts = lines[i].Split('	');
                if (parts.Length >= 2)
                {
                    dict[parts[1]] = parts[0];
                }
            }
        }
    }
}

