using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAXParser.Data
{
	class MethodData
	{
		private List<TagData> tags = new List<TagData>();

		public string Name { get; set; }
		public int Lines { get; set; }
		public List<TagData> Tags { get { return tags; } }
		
		
	}
}
