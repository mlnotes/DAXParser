using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DAXParser.zhfDataSetTableAdapters;
using DAXParser.CodeParse.Common;
using DAXParser.CodeParse.Form.Design;
using DAXParser.CodeParse.Form.DataSource;
using DAXParser.CodeParse.Form.DataSource.Field;

namespace DAXParser.CodeParse.Persistent
{
	class Dumper
	{
		protected static void DumpTag(int methodId, TagData tag)
		{
			tagTableAdapter table = new tagTableAdapter();
			table.Insert(tag.Name, tag.StartLine, tag.LineCount, methodId);
		}

		protected static void DumpMethod(int objectId, MethodData method)
		{
			methodTableAdapter table = new methodTableAdapter();
			int id = (int)table.InsertQuery(method.Name, method.LineCount, method.TagCount, objectId);
			
			foreach (TagData tag in method.Tags)
			{
				DumpTag(id, tag);
			}
		}

		public static void DumpClass(ClassData data)
		{
			objectTableAdapter table = new objectTableAdapter();
			int id = (int)table.InsertQuery(data.Name, "CLASS", data.Owner, data.LineCount, data.MethodCount, data.TagCount);

			foreach (MethodData method in data.Methods.Values)
			{
				DumpMethod(id, method);
			}
		}

		public static void DumpTable(TableData data)
		{
			objectTableAdapter table = new objectTableAdapter();
			int id = (int)table.InsertQuery(data.Name, "TABLE", data.Owner, data.LineCount, data.MethodCount, data.TagCount);

			foreach (MethodData method in data.Methods.Values)
			{
				DumpMethod(id, method);
			}
		}

		public static void DumpEnum(EnumData data)
		{
			objectTableAdapter table = new objectTableAdapter();
			table.InsertQuery(data.Name, "ENUM", data.Owner, data.LineCount, data.MethodCount, data.TagCount);
		}

		public static void DumpMap(MapData data)
		{
			objectTableAdapter table = new objectTableAdapter();
			int id = (int)table.InsertQuery(data.Name, "MAP", data.Owner, data.LineCount, data.MethodCount, data.TagCount);

			foreach (MethodData method in data.Methods.Values)
			{
				DumpMethod(id, method);
			}
		}

		public static void DumpQuery(QueryData data)
		{
			objectTableAdapter table = new objectTableAdapter();
			int id = (int)table.InsertQuery(data.Name, "QUERY", data.Owner, data.LineCount, data.MethodCount, data.TagCount);

			foreach (MethodData method in data.Methods.Values)
			{
				DumpMethod(id, method);
			}
		}

		public static void DumpForm(FormData data)
		{
			objectTableAdapter table = new objectTableAdapter();
			int id = (int)table.InsertQuery(data.Name, "FORM", data.Owner, data.LineCount, data.MethodCount, data.TagCount);

			foreach (MethodData method in data.Methods.Values)
			{
				DumpMethod(id, method);
			}

			// dump methods in control
			foreach (ControlData control in data.Controls.Values)
			{
				foreach (MethodData method in control.Methods.Values)
				{
					DumpMethod(id, method);
				}
			}

			// dump methods in datasource
			foreach (DataSourceData ds in data.DataSources.Values)
			{
				foreach (MethodData method in ds.Methods.Values)
				{
					DumpMethod(id, method);
				}

				foreach (DataFieldData field in ds.DataFields.Values)
				{
					foreach (MethodData method in field.Methods.Values)
					{
						DumpMethod(id, method);
					}
				}

				foreach (ReferenceFieldData field in ds.ReferenceFields.Values)
				{
					foreach (MethodData method in field.Methods.Values)
					{
						DumpMethod(id, method);
					}
				}
			}
		}
	}
}
