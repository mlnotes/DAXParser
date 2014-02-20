using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAXParser.CodeParse.Config
{
	class CountryTags
	{
		private static string[] TAGS = new string[]
		{
			 "GBR", "GIN", "GJP", "GCN", "GTH", 
			 "GEEU", "GTR", "GIL", "GEERU","GEECZ",
			 "GEELT","GEEPL","GEEW","GEELV"
		};

		public static bool IsCountryTag(string tag)
		{
			return TAGS.Contains(tag);
		}
	}
}
