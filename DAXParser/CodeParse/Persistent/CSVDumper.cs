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
		private string tempFile;
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
					tempFile = Path.Combine(Path.GetTempFileName());
					writer = new StreamWriter(tempFile);
					
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
				writer.Flush();
				writer.Dispose();

				using (StreamReader reader = new StreamReader(tempFile))
				{
					using (writer = new StreamWriter(output))
					{
						writer.Write("Name,Type,Prefix Owner,Postfix Owner,Region,Lines,Methods,Tags");
						foreach (string tag in tags)
						{
							writer.Write(",{0}", tag);
						}
						writer.WriteLine();

						while (!reader.EndOfStream)
						{
							writer.WriteLine(reader.ReadLine());
						}
					}
				}
			}
		}
	}
}
