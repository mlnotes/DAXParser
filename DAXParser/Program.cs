using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse;
using System.Text.RegularExpressions;
using DAXParser.CodeParse.DirParse;

namespace DAXParser
{
	class Program
	{
		static void Main(string[] args)
		{
			//string path = @"D:\code\source\Application\Foundation\SYP\Classes\Tax.xpo";
			//ClassData data = ClassData.Parse(path);
			//Console.WriteLine("Name:[{0}], Lines:[{1}]", data.Name, data.Lines);

			//string path = @"D:\code\source\Application\Foundation\SYP\Data Dictionary\Tables\WMSOrderTrans.xpo";
			//TableData data = TableData.Parse(path);
			//Console.WriteLine("Name:[{0}], Lines:[{1}]", data.Name, data.Lines);

			//string path = @"D:\code\source\Application\Foundation\SYP\Data Dictionary\Maps\AssetDepTransMap_JP.xpo";
			//MapData data = MapData.Parse(path);
			//Console.WriteLine("Name:[{0}], Mathods:[{1}], Lines:[{2}]", data.Name, data.Methods.Count, data.Lines);

			//string path = @"D:\code\source\Application\Foundation\SYP\Data Dictionary\Base Enums\DMFEntityType.xpo";
			//EnumData data = EnumData.Parse(path);
			//Console.WriteLine("Name:[{0}], Lines:[{1}]", data.Name, data.Lines);

			//string path = @"D:\code\source\Application\Foundation\SYP\Forms\ProjIntercompanyInvoice.xpo";
			//FormData data = FormData.Parse(path);
			//Console.WriteLine("Name:[{0}], Lines:[{1}]", data.Name, data.Lines);

			//string path = @"D:\code\source\Application\Foundation\SYP\Queries\VendInvoiceDocument.xpo";
			//QueryData data = QueryData.Parse(path);
			//Console.WriteLine("Name:[{0}], Mathods:[{1}], Lines:[{2}]", data.Name, data.Methods.Count, data.Lines);

			//ClassDirParser.Parse(new string[] { 
			//    @"D:\code\source\Application\sys",
			//    @"D:\code\source\Application\Foundation\SYP"
			//}, "*_CN.xpo");

			//TableDirParser.Parse(new string[] { 
			//    @"D:\code\source\Application\sys",
			//    @"D:\code\source\Application\Foundation\SYP"
			//}, "*_CN.xpo");

			//MapDirParser.Parse(new string[] { 
			//    @"D:\code\source\Application\sys",
			//    @"D:\code\source\Application\Foundation\SYP"
			//}, "*.xpo");

			//QueryDirParser.Parse(new string[] { 
			//    @"D:\code\source\Application\sys",
			//    @"D:\code\source\Application\Foundation\SYP"
			//}, "*_CN.xpo");

			//EnumDirParser.Parse(new string[] { 
			//    @"D:\code\source\Application\sys",
			//    @"D:\code\source\Application\Foundation\SYP"
			//}, "*_CN.xpo");

			FormDirParser.Parse(new string[] { 
			    @"D:\code\source\Application\sys",
			    @"D:\code\source\Application\Foundation\SYP"
			}, "*_CN.xpo");

			//string text = "CONTROL BUTTON #GeneralLedger";
			//int pos = text.LastIndexOf('#');
			//if (pos >= 0)
			//{
			//    Console.WriteLine("[{0}]", text.Substring(pos+1));
			//}
		}
	}
}
