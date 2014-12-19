using System;
using System.IO;
using System.Collections.Generic;

namespace Kaggle_HelpingSantasHelpers
{
	public static class DataReader
	{
		public const string filePath = @"/Volumes/DATA/Kaggle/HelpingSantasHelpers/toys_rev2.csv";
		private static FileStream inputFile;
		private static StreamReader reader;
		private static Queue<string> bufferedLine;
		private static bool hasReadHeader = false;
		private static int numberOfLinesRead = 0;
		private const int maxLinesToRead = 100000;

		public static StreamReader OpenReadStream (string path = filePath)
		{
			try {
				inputFile = File.OpenRead (path);
				reader = new StreamReader (inputFile);
				bufferedLine = new Queue<string> ();
				numberOfLinesRead = 0;
			} catch (FileNotFoundException ex) {
				Console.WriteLine ("In function 'OpenReadStream()'");
				Console.WriteLine (ex.Message);
				throw(ex);
			}

			return reader;
		}

		public static void CloseReadStream ()
		{
			try {
				reader.Close ();	
				hasReadHeader = false;
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}

		public static List<string> ReadNewIncomingOrdersFromStream (DateTime currentDate, int numberOfDaysAhead = 1)
		{
			List<string> lines = new List<string> ();

			while (!reader.EndOfStream && numberOfLinesRead <= maxLinesToRead) {
				if (!IsWithinNumberOfDays (currentDate, numberOfDaysAhead)) {
					break;
				}
				string line = ReadNextLine ();
				lines.Add (line);
			}
			PrintLinesSummary (lines);

			return lines;
		}

		public static List<string> ReadLinesFromStream (int linesToRead = maxLinesToRead)
		{
			List<string> lines = new List<string> ();

			for (int i = 0; i < linesToRead; i++) {
				if (!reader.EndOfStream) {
					string line = reader.ReadLine ();
					lines.Add (line);
				} else {
					Console.WriteLine ("End of file reached!");
					break;
				}
			}
			PrintLinesSummary (lines);

			return lines;
		}


		private static string ReadNextLine ()
		{
			string line = null;

			if (bufferedLine.Count > 0) {
				line = bufferedLine.Dequeue ();
			} else if (!reader.EndOfStream) {
				line = reader.ReadLine ();
			}
			numberOfLinesRead++;

			return line;
		}

		private static string PeekNextLine ()
		{
			string line = null;

			if (bufferedLine.Count > 0) {
				line = bufferedLine.Peek ();
			} else if (!reader.EndOfStream) {
				line = reader.ReadLine ();
				bufferedLine.Enqueue (line);
			}

			return line;
		}

		public static string ReadHeaderLine ()
		{
			if (!hasReadHeader) {
				string header = reader.ReadLine ();
				hasReadHeader = true;
				return header;
			} else {
				return "";
			}
		}

		private static void PrintLinesSummary (List<string> lines)
		{
			// Write lines to console for testing
			int linesCount = lines.Count;
			int firstLines = 3;
			int lastLines = 3;

			// First lines
			if (linesCount >= firstLines) {
				for (int i = 0; i < firstLines; i++) {
					Console.WriteLine (lines [i]);
				}
			} else {
				for (int i = 0; i < linesCount; i++) {
					Console.WriteLine (lines [i]);
				}
			}

			// Dots
			if (linesCount >= firstLines + lastLines + 1) {
				for (int i = 0; i < 3; i++) {
					Console.WriteLine ("   .");
				}
			}

			// Last lines
			if (linesCount >= firstLines + lastLines) {
				for (int i = linesCount - lastLines; i < linesCount; i++) {
					Console.WriteLine (lines [i]);
				}
			} else if (linesCount > firstLines) {
				for (int i = linesCount - (linesCount - firstLines); i < linesCount; i++) {
					Console.WriteLine (lines [i]);
				}
			}
					
			Console.WriteLine ("---");
			Console.WriteLine (String.Format ("N = {0} ", linesCount));
		}

		private static bool IsWithinNumberOfDays (DateTime currentDate, int numberOfDaysAhead)
		{
			bool isWithinNumberOfDays = false;
			string nextLine = PeekNextLine ();
			DateTime dateNextLine = ParseDateFromLine (nextLine);
			TimeSpan timeBetween = dateNextLine - currentDate;

			if (timeBetween.TotalDays < numberOfDaysAhead) {
				isWithinNumberOfDays = true;
			} else {
				isWithinNumberOfDays = false;
			}

			return isWithinNumberOfDays;
		}

		private static DateTime ParseDateFromLine (string line)
		{
			DateTime date = new DateTime ();
			string[] values = line.Split (',');
			string[] dateComponents = values [1].Split (' ');

			try {
				int year = Convert.ToInt32 (dateComponents [0]);
				int month = Convert.ToInt32 (dateComponents [1]);
				int day = Convert.ToInt32 (dateComponents [2]);
				int hour = Convert.ToInt32 (dateComponents [3]);
				int minute = Convert.ToInt32 (dateComponents [4]);
				int second = 0;

				date = new DateTime (year, month, day, hour, minute, second);

			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}

			return date;
		}
	}
}

