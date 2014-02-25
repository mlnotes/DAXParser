using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.Data.Form.DataSource.Field;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse;

namespace DAXParser.Data.Form.DataSource
{
	class DataSourceData : BaseObjectData
	{
		private Dictionary<string, DataFieldData> dataFields = new Dictionary<string, DataFieldData>();
		private Dictionary<string, ReferenceFieldData> referenceFields = new Dictionary<string, ReferenceFieldData>();

		public Dictionary<string, DataFieldData> DataFields { get { return dataFields; } }
		public Dictionary<string, ReferenceFieldData> ReferenceFields { get { return referenceFields; } }

		public void AddDataField(DataFieldData field)
		{
			string name = field.Name.ToUpper();
			if (dataFields.Keys.Contains(name))
			{
				Lines -= dataFields[name].Lines;
			}

			dataFields.Add(name, field);
			Lines += field.Lines;
		}

		public void AddReferenceField(ReferenceFieldData field)
		{
			string name = field.Name.ToUpper();
			if (referenceFields.Keys.Contains(name))
			{
				Lines -= referenceFields[name].Lines;
			}

			referenceFields.Add(name, field);
			Lines += field.Lines;
		}

		private static void ParseFieldList(StreamReader reader, DataSourceData data)
		{
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith(KeyWords.DATAFIELD))
				{
					data.AddDataField(DataFieldData.Parse(line, reader));
				}
				else if (line.StartsWith(KeyWords.REFERENCEFIELD))
				{
					data.AddReferenceField(ReferenceFieldData.Parse(line, reader));
				}
				else if (line.StartsWith(KeyWords.ENDFIELDLIST))
				{
					break;
				}
			}
		}

		public static DataSourceData Parse(StreamReader reader)
		{
			DataSourceData data = new DataSourceData();

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith(KeyWords.ENDDATASOURCE))
				{
					break;
				}
				else if (line.StartsWith(KeyWords.Name))
				{
					data.Name = CoreParser.GetName(line, KeyWords.Name);
				}
				else if (line.StartsWith(KeyWords.METHODS))
				{
					List<MethodData> methods = CoreParser.ParseMethods(reader);
					foreach (MethodData m in methods)
					{
						data.AddMethod(m);
					}
				}
				else if (line.StartsWith(KeyWords.FIELDLIST))
				{
					ParseFieldList(reader, data);
				}
				else if (line.StartsWith(KeyWords.ENDDATASOURCE))
				{
					break;
				}
			}

			return data;
		}
	}
}
