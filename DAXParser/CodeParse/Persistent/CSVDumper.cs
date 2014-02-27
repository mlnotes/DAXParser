using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse.Common;
using System.IO;
using System.Runtime.CompilerServices;

namespace DAXParser.CodeParse.Persistent
{
	class CSVDumper: IDisposable
	{
		private string output;
		private StreamWriter writer;
		private List<string> tags;
		private static CSVDumper instance;
		

		private CSVDumper()
		{
			Output = "dax_parse_result.csv";
			tags = new List<string>();
		}

		public static CSVDumper GetInstance()
		{
			if (instance == null)
			{
				instance = new CSVDumper();
			}
			return instance;
		}

		public string Output
		{
			set
			{
				if (output == value)
				{
					return;
				}
				else
				{
					output = value;
					writer = new StreamWriter(output);
					//writer.WriteLine("Name,Type,Owner,Lines,Methods,Tags,Lines Of Tags");
				}
			}

			get
			{
				return output;
			}
		}

		public void Dump<T>(List<T> data)
			where T: BaseObjectData
		{
			if (writer == null)
			{
				return;
			}

			foreach (T d in data)
			{
				d.Dump(writer, tags);
			}
		}

		public void Dispose()
		{
			if (writer != null)
			{
				writer.Write("Name,Type,Owner,Lines,Methods,Tags");
				foreach (string tag in tags)
				{
					writer.Write(",{0}", tag);
				}
				writer.WriteLine();

				writer.Flush();
				writer.Dispose();
			}
		}
	}
}
