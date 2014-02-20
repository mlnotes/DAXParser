using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse.Data;
using DAXParser.CodeParse;

namespace DAXParser
{
	class Program
	{
		static void Main(string[] args)
		{
			string path = @"D:\code\source\Application\Foundation\SYP\Classes\Tax.xpo";
			ClassData cd = CoreParser.ParseClass(path);
			Console.WriteLine("Name:[{0}], Lines:[{1}]", cd.Name, cd.Lines);
		}
	}
}
