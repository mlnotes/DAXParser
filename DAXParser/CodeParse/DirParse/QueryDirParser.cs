using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.Persistent;

namespace DAXParser.CodeParse.DirParse
{
	class QueryDirParser: BaseDirParser
	{
		public static void Parse(string[] layerPaths, string moduleDir, Dictionary<string, string> ownership, string pattern = "*.xpo")
		{
			Parse(layerPaths, moduleDir, ownership, QueryData.Parse, Dumper.DumpQuery, pattern);
		}
	}
}
