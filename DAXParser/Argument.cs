using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DAXParser
{
    class Argument
    {
        private static string DIRS = "-dirs";
        private static string MODULES = "-modules";
        private static string OWNERSHIP = "-ownership";

        private List<string> dirs;
        private List<string> modules;
        private Dictionary<string, string> ownership;
        
        private Argument()
        {
            dirs = new List<string>();
            modules = new List<string>();
            ownership = new Dictionary<string, string>();
        }

        public List<string> Dirs
        {
            get { return dirs; }
        }

        public List<string> Modules
        {
            get { return modules; }
        }

        public Dictionary<string, string> Ownership
        {
            get { return ownership; }
        }

        public static Argument parse(string[] args)
        {
            Argument argument = new Argument();
            int i = 0;
            while (i < args.Length)
            {
                if (args[i] == DIRS)
                {
                    while (++i < args.Length && !args[i].StartsWith("-"))
                    {
                        argument.dirs.Add(args[i]);
                    }
                }
                else if(args[i] == MODULES)
                {
                    while (++i < args.Length && !args[i].StartsWith("-"))
                    {
                        argument.modules.Add(args[i]);
                    }
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

