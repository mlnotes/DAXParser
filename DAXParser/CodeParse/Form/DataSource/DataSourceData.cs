using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse.Form.DataSource.Field;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse;
using DAXParser.CodeParse.Common;

namespace DAXParser.CodeParse.Form.DataSource
{
	class DataSourceData : BaseObjectData
	{
		private Dictionary<string, DataFieldData> dataFields = new Dictionary<string, DataFieldData>();
		private Dictionary<string, ReferenceFieldData> referenceFields = new Dictionary<string, ReferenceFieldData>();

		public Dictionary<string, DataFieldData> DataFields { get { return dataFields; } }
		public Dictionary<string, ReferenceFieldData> ReferenceFields { get { return referenceFields; } }

		public override int MethodCount
		{
			get
			{
				int methodCount = methods.Count;
				foreach (DataFieldData field in dataFields.Values)
				{
					methodCount += field.MethodCount;
				}

				foreach (ReferenceFieldData field in referenceFields.Values)
				{
					methodCount += field.MethodCount;
				}

				return methodCount;
			}
		}

		public override int TagCount
		{
			get
			{
				int tagCount = this.tagCount;
				foreach (DataFieldData field in dataFields.Values)
				{
					tagCount += field.TagCount;
				}

				foreach (ReferenceFieldData field in referenceFields.Values)
				{
					tagCount += field.TagCount;
				}

				return tagCount;
			}
		}

		public void AddDataField(DataFieldData field)
		{
			string name = field.Name.ToUpper();
			if (dataFields.Keys.Contains(name))
			{
				lineCount -= dataFields[name].LineCount;
				dataFields[name].MergeWith(field);
				lineCount += dataFields[name].LineCount;
			}
			else
			{
				dataFields.Add(name, field);
				lineCount += field.LineCount;
			}
		}

		public void AddReferenceField(ReferenceFieldData field)
		{
			string name = field.Name.ToUpper();
			if (referenceFields.Keys.Contains(name))
			{
				lineCount -= referenceFields[name].LineCount;
				referenceFields[name].MergeWith(field);
				lineCount += referenceFields[name].LineCount;
			}
			else
			{
				referenceFields.Add(name, field);
				lineCount += field.LineCount;
			}
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
					data.Name = line.Substring(KeyWords.Name.Length).Trim().Substring(1);
				}
				else if (line.StartsWith(KeyWords.METHODS))
				{
					List<MethodData> methods = ParseMethods(reader);
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

		public override BaseObjectData MergeWith(BaseObjectData data)
		{
			DataSourceData dsData = (DataSourceData)data;

			// merge methods
			foreach (MethodData m in dsData.Methods.Values)
			{
				this.AddMethod(m);
			}

			// merge data fields
			foreach (DataFieldData field in dsData.DataFields.Values)
			{
				this.AddDataField(field);
			}

			// merge reference fields
			foreach (ReferenceFieldData field in dsData.ReferenceFields.Values)
			{
				this.AddReferenceField(field);
			}

			return this;
		}
	}
}
