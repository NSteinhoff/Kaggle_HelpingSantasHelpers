using System;

namespace Kaggle_HelpingSantasHelpers
{
	public static class DateParser
	{
		public static DateTime ParseDateFromLine (string line)
		{
			DateTime date = new DateTime ();
			string[] values = line.Split (',');
			string[] dateComponents = values [1].Split (' ');

			date = ParseDateFromStringArray (dateComponents);

			return date;
		}

		private static DateTime ParseDateFromStringArray (string[] dateComponents)
		{
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
				throw new Exception ("Could not parse date!", ex);
			}
		}
	}
}

