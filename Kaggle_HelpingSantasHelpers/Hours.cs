using System;

namespace Kaggle_HelpingSantasHelpers
{
	public static class Hours
	{
		public static DateTime NextMorning (DateTime time)
		{
			return new DateTime (time.Year, time.Month, time.Day, 9, 0, 0).AddDays (1);
		}

		public static DateTime StartOfWorkday (DateTime time)
		{
			return new DateTime (time.Year, time.Month, time.Day, 9, 0, 0);
		}

		public static DateTime EndOfWorkday (DateTime time)
		{
			return new DateTime (time.Year, time.Month, time.Day, 19, 0, 0);
		}

		public static int MinutesLeftInWorkday (DateTime time)
		{
			return (int)Math.Min (Math.Max ((EndOfWorkday (time) - time).TotalMinutes, 0), 600);
		}

		public static TimeSpan TimespanLeftInWorkday (DateTime time)
		{
			return EndOfWorkday (time) - time;
		}

		public static DateTime NextSanctionedMinute (DateTime time)
		{
			DateTime nextSanctionedMinute;
			DateTime startOfWorkday = StartOfWorkday (time);
			DateTime endOfWorkday = EndOfWorkday (time);
			DateTime nextMorning = NextMorning (time);

			if (time < startOfWorkday) {
				nextSanctionedMinute = startOfWorkday;
			} else if (time >= endOfWorkday) {
				nextSanctionedMinute = nextMorning;
			} else {
				nextSanctionedMinute = time;
			}

			return nextSanctionedMinute;
		}

		public static DateTime Duplicate (DateTime time)
		{
			return new DateTime (time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);

		}
	}
}

