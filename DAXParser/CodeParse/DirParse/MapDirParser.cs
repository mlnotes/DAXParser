﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse.Config;

namespace DAXParser.CodeParse.DirParse
{
	class MapDirParser:BaseDirParser
	{
		public static void Parse(string[] layerPaths, string moduleDir, string pattern = "*.xpo")
		{
			Parse(layerPaths, moduleDir, MapData.Parse, pattern);
		}
	}
}
