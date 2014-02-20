using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAXParser.CodeParse.Data
{
	class ClassData
	{
		private List<MethodData> methods = new List<MethodData>();

		public string Name { get; set; }
		public int Lines { get; set; }
		public List<MethodData> Methods { get { return methods; } }
	}
}
