using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DAXParser.CodeParse.DirParse
{
	class ClassDirParser
	{
		public static void Parse(string path, string pattern = "*.xpo")
		{
			DirectoryInfo dir = new DirectoryInfo(path);
			FileInfo[] files = dir.GetFiles(pattern);
			foreach (FileInfo file in files)
			{
				ClassData data = ClassData.Parse(file.FullName);
				Console.WriteLine("[{0}],[{1}],[{2}]", data.Name, data.Methods.Count, data.Lines);
			}
		}
	}
}
