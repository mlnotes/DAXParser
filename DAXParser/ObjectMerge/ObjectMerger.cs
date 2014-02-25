using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAXParser.CodeParse;

namespace DAXParser.ObjectMerge
{
	class ObjectMerger
	{
		public static ClassData MergeToFirstClass(List<ClassData> clazzes)
		{
			ClassData result = clazzes[0];

			for (int i = 1; i < clazzes.Count; ++i)
			{
				foreach (KeyValuePair<string, MethodData> pair in clazzes[i].Methods)
				{
					result.AddMethod(pair.Value);
				}
			}

			return result;
		}
	}
}
