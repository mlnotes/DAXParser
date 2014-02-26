using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.Persistent;

namespace DAXParser.CodeParse.DirParse
{
	class EnumDirParser:BaseDirParser
	{
		public static void Parse(string[] layerPaths, string moduleDir, string pattern = "*.xpo")
		{
			Parse(layerPaths, moduleDir, EnumData.Parse, Dumper.DumpEnum, pattern);
		}
	}
}
