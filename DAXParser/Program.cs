using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.Data;
using DAXParser.CodeParse;
using System.Text.RegularExpressions;

namespace DAXParser
{
	class Program
	{
		static void Main(string[] args)
		{
			//string path = @"D:\code\source\Application\Foundation\SYP\Classes\Tax.xpo";
			//ClassData cd = CoreParser.ParseClass(path);
			//Console.WriteLine("Name:[{0}], Lines:[{1}]", cd.Name, cd.Lines);

			//string path = @"D:\code\source\Application\Foundation\SYP\Data Dictionary\Tables\TaxTrans.xpo";
			//TableData table = CoreParser.ParseTable(path);
			//Console.WriteLine("Name:[{0}], Lines:[{1}]", table.Name, table.Lines);

			string path = @"D:\code\source\Application\Foundation\SYP\Data Dictionary\Base Enums\CustVendSettlementOffsetVoucherType.xpo";
			EnumData data = CoreParser.ParseEnum(path);
			Console.WriteLine("Name:[{0}], Lines:[{1}]", data.Name, data.Lines);
			
		}
	}
}
