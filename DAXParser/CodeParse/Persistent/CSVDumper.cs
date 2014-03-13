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
		private Dictionary<string, string> tagRegion;
		private string[] tagRegionName;

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

		public Dictionary<string, string> TagRegion
		{
			get
			{
				return tagRegion;
			}
			set
			{
				tagRegion = value;
				HashSet<string> uniqueRegions = new HashSet<string>();
				foreach (string region in tagRegion.Values)
				{
					uniqueRegions.Add(region);
				}

				tagRegionName = new string[uniqueRegions.Count];
				int index = 0;
				foreach (string region in uniqueRegions)
				{
					tagRegionName[index++] = region;
				}
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
				d.Dump(writer, tags, tagRegion, tagRegionName);
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
						writer.Write("Name,Type,Region,Owner,Prefix Owner,Postfix Owner,Country,Lines,Methods,Tags");
						
						// tag region is fixed
						if (tagRegionName != null && tagRegionName.Length > 0)
						{
							foreach (string region in tagRegionName)
							{
								writer.Write(",{0}", region);
							}
						}

						
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
