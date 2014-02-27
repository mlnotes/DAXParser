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
		private static CSVDumper instance;

		private CSVDumper()
		{
			Output = "dax_parse_result.csv";
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
					writer.WriteLine("Name,Type,Owner,Line Count, Method Count, Tag Count");
				}
			}

			get
			{
				return output;
			}
		}

		public void DumpClass(List<ClassData> data)
		{
			foreach (ClassData d in data)
			{
				DumpObject(d);
			}
		}

		public void DumpTable(List<TableData> data)
		{
			foreach (TableData d in data)
			{
				DumpObject(d);
			}
		}

		public void DumpMap(List<MapData> data)
		{
			foreach (MapData d in data)
			{
				DumpObject(d);
			}
		}

		public void DumpQuery(List<QueryData> data)
		{
			foreach (QueryData d in data)
			{
				DumpObject(d);
			}
		}

		public void DumpEnum(List<EnumData> data)
		{
			foreach (EnumData d in data)
			{
				DumpObject(d);
			}
		}

		public void DumpForm(List<FormData> data)
		{
			foreach (FormData d in data)
			{
				DumpObject(d);
			}
		}

		private void DumpObject<T>(T data)
			where T : BaseObjectData
		{
			if (writer == null)
			{
				return;
			}

			writer.WriteLine("{0},{1},{2},{3},{4},{5}", data.Name, data.Type, data.Owner,
				data.LineCount, data.MethodCount, data.TagCount);
		}

		public void Dispose()
		{
			if (writer != null)
			{
				writer.Flush();
				writer.Dispose();
			}
		}
	}
}
