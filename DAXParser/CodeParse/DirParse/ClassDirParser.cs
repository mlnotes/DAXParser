using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse.Persistent;

namespace DAXParser.CodeParse.DirParse
{
	class ClassDirParser : BaseDirParser
	{
		public static void Parse(string[] layerPaths, string moduleDir, string pattern = "*.xpo")
		{
			Parse(layerPaths, moduleDir, ClassData.Parse, Dumper.DumpClass, pattern);
		}
	}
}