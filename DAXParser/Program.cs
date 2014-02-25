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
			//ClassData data = CoreParser.ParseClass(path);
			//Console.WriteLine("Name:[{0}], Lines:[{1}]", data.Name, data.Lines);

			//string path = @"D:\code\source\Application\Foundation\SYP\Data Dictionary\Tables\TaxTrans.xpo";
			//TableData data = CoreParser.ParseTable(path);
			//Console.WriteLine("Name:[{0}], Lines:[{1}]", data.Name, data.Lines);

			//string path = @"D:\code\source\Application\Foundation\SYP\Data Dictionary\Base Enums\CustVendSettlementOffsetVoucherType.xpo";
			//EnumData data = CoreParser.ParseEnum(path);
			//Console.WriteLine("Name:[{0}], Lines:[{1}]", data.Name, data.Lines);

			string path = @"D:\code\source\Application\Foundation\SYP\Forms\ProjIntercompanyInvoice.xpo";
			FormData data = FormData.Parse(path);
			Console.WriteLine("Name:[{0}], Lines:[{1}]", data.Name, data.Lines);
			
		}
	}
}
