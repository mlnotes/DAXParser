﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;

namespace DAXParser.CodeParse.DirParse
{
	class ClassDirParser : BaseDirParser
	{
		public static void Parse(string[] layerPaths, string pattern = "*.xpo")
		{
			Parse(layerPaths, ModuleDirs.CLASS, ClassData.Parse, pattern);
		}
	}
}