using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAXParser.CodeParse.Common
{
	class TagData
	{
		private bool isOpen;
		private string name;
		private int lineNo;

		public TagData(string tag, int lineNo)
		{
			this.lineNo = lineNo;
			if (tag.StartsWith("/"))
			{
				isOpen = false;
				name = tag.Substring(1);
			}
			else
			{
				isOpen = true;
				name = tag;
			}
		}

		public string Name { get { return name; } }
		public int LineNo { get { return lineNo; } }
		public bool Open { get { return isOpen; } }
		public bool Close { get { return !isOpen; } }
	}
}
