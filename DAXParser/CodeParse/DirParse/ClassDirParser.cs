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
			List<ClassData> clazzes = new List<ClassData>();
			int methods = 0;
			int lines = 0;
			foreach (FileInfo file in files)
			{
				ClassData data = ClassData.Parse(file.FullName);
				//Console.WriteLine("[{0}],[{1}],[{2}]", data.Name, data.Methods.Count, data.Lines);

				clazzes.Add(data);
				methods += data.Methods.Count;
				lines += data.Lines;
			}

			Console.WriteLine("Objects: [{0}]\nMethods: [{1}]\nLines: [{2}]", clazzes.Count, methods, lines);
		}


	}
}
