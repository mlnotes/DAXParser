using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAXParser.Data
{
	class ClassData
	{
		private Dictionary<string, MethodData> methods = new Dictionary<string, MethodData>();

		public string Name { get; set; }
		public int Lines { get; set; }
		
		public void AddMethod(MethodData method)
		{
			string name = method.Name.ToUpper();
			if (methods.Keys.Contains(name))
			{
				Lines -= methods[name].Lines;
			}
			
			methods.Add(method.Name.ToUpper(), method);
			Lines += method.Lines;
		}

		public Dictionary<string, MethodData> Methods { get { return methods; } }
	}
}
