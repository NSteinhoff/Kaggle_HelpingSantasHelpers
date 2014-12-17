using System;
using System.IO;
using System.Collections.Generic;

namespace Kaggle_HelpingSantasHelpers
{
	public static class DataReader
	{
		#region Fields

		private static FileStream inputFile;

		private static StreamReader reader;

		private static Queue<string> bufferedLine;

		#endregion


		#region Public methods

		public static void Testing ()
		{
			DateTime currentDate = new DateTime (2014, 1, 1, 0, 0, 0);

			List<string> newOrders = ReadNewIncomingOrdersFromStream (currentDate);

			ToyOrderBook.AddNewOrdersToOrderBook (newOrders);
		}

		public static bool OpenReadStream (string path)
		{
			try {
				inputFile = File.OpenRead (path);

				reader = new StreamReader (inputFile);

				bufferedLine = new Queue<string> ();

				Console.WriteLine (String.Format ("Header - {0}", ReadHeaderLine ()));

			} catch (Exception ex) {
				Console.WriteLine (ex.Message);

				return false;
			}
				

			return true;
		}


		public static void CloseReadStream ()
		{
			try {
				reader.Close ();
				
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}

		public static List<string> ReadNewIncomingOrdersFromStream (DateTime currentDate, int numberOfDaysAhead = 1)
		{
			List<string> lines = new List<string> ();

			while (!reader.EndOfStream) {

				if (!IsWithinNumberOfDays (currentDate, numberOfDaysAhead)) {
					break;
				}

				string line = ReadNextLine ();

				lines.Add (line);
			}

			PrintLinesSummary (lines);

			return lines;
		}

		public static List<string> ReadLinesFromStream (int linesToRead = 10000)
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

		#endregion


		#region Private methods

		private static string ReadNextLine ()
		{
			string line = null;

			if (bufferedLine.Count > 0) {
				line = bufferedLine.Dequeue ();

			} else if (!reader.EndOfStream) {
				line = reader.ReadLine ();

			}

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

		private static string ReadHeaderLine ()
		{
			string header = reader.ReadLine ();

			return header;
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
			string nextLine = PeekNextLine ();

			DateTime dateNextLine = ParseDateFromLine (nextLine);

			TimeSpan timeBetween = dateNextLine - currentDate;

			if (timeBetween.TotalDays < numberOfDaysAhead) {
				return true;
			}

			return false;
		}

		private static DateTime ParseDateFromLine (string line)
		{
			string[] values = line.Split (',');

			string[] dateComponents = values [1].Split (' ');

			try {
				int year = Convert.ToInt32 (dateComponents [0]);
				int month = Convert.ToInt32 (dateComponents [1]);
				int day = Convert.ToInt32 (dateComponents [2]);
				int hour = Convert.ToInt32 (dateComponents [3]);
				int minute = Convert.ToInt32 (dateComponents [4]);
				int second = 0;

				DateTime date = new DateTime (year, month, day, hour, minute, second);

				return date;
				
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);

				return new DateTime ();
			}
		}

		#endregion
	}
}

