using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse.Form.DataSource;
using DAXParser.CodeParse.Form.Design;
using System.IO;
using DAXParser.CodeParse.Config;
using DAXParser.CodeParse;
using DAXParser.CodeParse.Common;
using DAXParser.CodeParse.Form.DataSource.Field;
using DAXParser.CodeParse.IO;

namespace DAXParser.CodeParse
{
	class FormData: BaseObjectData
	{
		public override string Type
		{
			get { return "FORM"; }
		}

		private Dictionary<string, DataSourceData> dataSources = new Dictionary<string,DataSourceData>();
		private Dictionary<string, ControlData> controls = new Dictionary<string,ControlData>();

		public Dictionary<string, DataSourceData> DataSources { get { return dataSources; } }
		public Dictionary<string, ControlData> Controls { get { return controls; } }

		public override int MethodCount
		{
			get
			{
				int methodCount = methods.Count;
				foreach (DataSourceData ds in dataSources.Values)
				{
					methodCount += ds.MethodCount;
				}

				foreach (ControlData control in controls.Values)
				{
					methodCount += control.MethodCount;
				}

				return methodCount;
			}
		}

		public override int TagCount
		{
			get
			{
				int tagCount = this.tagCount;
				foreach (DataSourceData ds in dataSources.Values)
				{
					tagCount += ds.TagCount;
				}

				foreach (ControlData control in controls.Values)
				{
					tagCount += control.TagCount;
				}

				return tagCount;
			}
		}

		public void AddDataSource(DataSourceData dataSource)
		{
			string name = dataSource.Name.ToUpper();
			if (dataSources.Keys.Contains(name))
			{
				lineCount -= dataSources[name].LineCount;
				dataSources[name].MergeWith(dataSource);
				lineCount += dataSources[name].LineCount;
			}
			else
			{
				dataSources[name] = dataSource;
				lineCount += dataSource.LineCount;
			}
		}

		public void AddControl(ControlData control)
		{
			string name = control.Name.ToUpper();
			if (controls.Keys.Contains(name))
			{
				lineCount -= controls[name].LineCount;
				controls[name].MergeWith(control);
				lineCount += controls[name].LineCount;
			}
			else
			{
				controls[name] = control;
				lineCount += control.LineCount;
			}
			
		}

		private static void ParseDesign(XPOReader reader, FormData data)
		{
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith(KeyWords.CONTROL))
				{
					data.AddControl(ControlData.Parse(line, reader));
				}
				else if (line.StartsWith(KeyWords.ENDDESIGN))
				{
					break;
				}
			}
		}

		private static void ParseDataSources(XPOReader reader, FormData data)
		{
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine().TrimStart();
				if (line.StartsWith(KeyWords.DATASOURCE))
				{
					data.AddDataSource(DataSourceData.Parse(reader));
				}
				else if (line.StartsWith(KeyWords.ENDOBJECTBANK))
				{
					break;
				}
			}
		}

		public static FormData Parse(string path)
		{
			XPOReader reader;
			FormData data = new FormData();
			using (reader = new XPOReader(path))
			{
				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimStart();
					if (string.IsNullOrEmpty(data.Name) && line.StartsWith(KeyWords.FORM))
					{
						data.Name = line.Substring(KeyWords.FORM.Length).Trim().Substring(1);
					}
					else if (line.StartsWith(KeyWords.ENDFORM))
					{
						break;
					}
					else if (line.StartsWith(KeyWords.METHODS))
					{
						List<MethodData> methods = ParseMethods(reader);
						foreach (MethodData m in methods)
						{
							data.AddMethod(m);
						}
					}
					else if (line.StartsWith(KeyWords.DESIGN))
					{
						ParseDesign(reader, data);
					}
					else if (line.StartsWith(KeyWords.OBJECTBANK))
					{
						ParseDataSources(reader, data);
					}
				}
			}

			data.LineCountOfFile = reader.LineCountOfFile;
			return data;
		}

		public override BaseObjectData MergeWith(BaseObjectData data)
		{
			FormData fData = (FormData)data;

			// merge methods
			foreach (MethodData m in fData.Methods.Values)
			{
				this.AddMethod(m);
			}

			// merge controls
			foreach (ControlData control in fData.Controls.Values)
			{
				this.AddControl(control);
			}

			// merge data sources
			foreach (DataSourceData ds in fData.DataSources.Values)
			{
				this.AddDataSource(ds);
			}

			return this;
		}

		private void GetTagInfoFromMethod(Dictionary<string, int> tags, MethodData method)
		{
			foreach (TagData tag in method.Tags)
			{
				if (tags.ContainsKey(tag.Name))
				{
					tags[tag.Name] += tag.LineCount;
				}
				else
				{
					tags[tag.Name] = tag.LineCount;
				}
			}
		}

		protected override Dictionary<string, int> GetTagInfo()
		{
			Dictionary<string, int> tags = new Dictionary<string, int>();
			
			// methods in form
			foreach (MethodData m in methods.Values)
			{
				GetTagInfoFromMethod(tags, m);
			}

			// methods in control
			foreach (ControlData control in controls.Values)
			{
				foreach (MethodData m in control.Methods.Values)
				{
					GetTagInfoFromMethod(tags, m);
				}
			}

			// methods in data source
			foreach (DataSourceData ds in dataSources.Values)
			{
				foreach (MethodData m in ds.Methods.Values)
				{
					GetTagInfoFromMethod(tags, m);
				}

				// methods in data field
				foreach (DataFieldData field in ds.DataFields.Values)
				{
					foreach (MethodData m in field.Methods.Values)
					{
						GetTagInfoFromMethod(tags, m);
					}
				}

				// methods in reference field
				foreach (ReferenceFieldData field in ds.ReferenceFields.Values)
				{
					foreach (MethodData m in field.Methods.Values)
					{
						GetTagInfoFromMethod(tags, m);
					}
				}
			}


			return tags;
		}
	}
}
