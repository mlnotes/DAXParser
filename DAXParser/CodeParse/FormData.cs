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

namespace DAXParser.CodeParse
{
	class FormData: BaseObjectData
	{
		private Dictionary<string, DataSourceData> dataSources = new Dictionary<string,DataSourceData>();
		private Dictionary<string, ControlData> controls = new Dictionary<string,ControlData>();

		public void AddDataSource(DataSourceData dataSource)
		{
			string name = dataSource.Name.ToUpper();
			if (dataSources.Keys.Contains(name))
			{
				Lines -= dataSource.Lines;
			}
			dataSources[name] = dataSource;
			Lines += dataSource.Lines;
		}

		public void AddControl(ControlData control)
		{
			string name = control.Name.ToUpper();
			if(controls.Keys.Contains(name))
			{
				Lines -= control.Lines;
			}
			controls[name] = control;
			Lines += control.Lines;
		}

		private static void ParseDesign(StreamReader reader, FormData data)
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

		private static void ParseDataSources(StreamReader reader, FormData data)
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
			using (StreamReader reader = new StreamReader(path))
			{
				FormData data = new FormData();

				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimStart();
					if (line.StartsWith(KeyWords.FORM))
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

				return data;
			}
		}
	}
}
