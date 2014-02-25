﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAXParser.CodeParse.Config;

namespace DAXParser.CodeParse.DirParse
{
	class TableDirParser:BaseDirParser
	{
		public static void Parse(string[] layerPaths, string moduleDir, string pattern = "*.xpo")
		{
			Parse(layerPaths, moduleDir, TableData.Parse, pattern);
		}
	}
}
