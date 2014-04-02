using System;
using System.IO;

namespace DAXParser.CodeParse.IO
{
	class XPOReader : IDisposable
	{
		private StreamReader reader;
		private int lineCountOfFile = 0;

		public int LineCountOfFile { get { return lineCountOfFile; } }
		public Boolean EndOfStream { get { return reader.EndOfStream; } }
		
		public XPOReader(string path)
		{
			reader = new StreamReader(path);
		}

		public string ReadLine()
		{
			string line = reader.ReadLine();
			if (line != null)
			{
				lineCountOfFile++;
			}
			return line;
		}

		public void Dispose()
		{
			while (!reader.EndOfStream)
			{
				reader.ReadLine();
				lineCountOfFile++;
			}
			reader.Dispose();
		}
	}
}
