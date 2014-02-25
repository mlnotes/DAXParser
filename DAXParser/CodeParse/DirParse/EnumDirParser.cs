using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse.Config;

namespace DAXParser.CodeParse.DirParse
{
	class EnumDirParser:BaseDirParser
	{
		public static void Parse(string[] layerPaths, string pattern = "*.xpo")
		{
			Parse(layerPaths, ModuleDirs.ENUM, EnumData.Parse, pattern);
		}
	}
}
