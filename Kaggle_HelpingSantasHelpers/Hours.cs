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
			TimeSpan timeSpanLeft;

			if (time > EndOfWorkday (time)) {
				timeSpanLeft = new TimeSpan (0);
			} else if (time < StartOfWorkday (time)) {
				timeSpanLeft = EndOfWorkday (time) - StartOfWorkday (time);
			} else {
				timeSpanLeft = EndOfWorkday (time) - time;
			}
				
			return timeSpanLeft;
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

		public static int CalculateSanctionedMinutes (DateTime startTime, int durationInMinutes)
		{
			DateTime end = startTime.AddMinutes (durationInMinutes);

			int sanctionedMinutesStartDay = MinutesLeftInWorkday (startTime);

			int fullDays = (int)Math.Floor ((end - startTime).TotalDays);
			int remainingMinutes = (int)(end - startTime.AddDays (fullDays)).TotalMinutes;

			int sanctionedMinutesFromFullDays = 10 * 60 * fullDays;
			int sanctionedMinutesFromPartialDays = 0;

			if (remainingMinutes <= sanctionedMinutesStartDay) {
				sanctionedMinutesFromPartialDays = remainingMinutes;
			} else if (remainingMinutes <= (sanctionedMinutesStartDay + 14 * 60)) {
				sanctionedMinutesFromPartialDays = sanctionedMinutesStartDay;
			} else {
				sanctionedMinutesFromPartialDays = remainingMinutes - (14 * 60);
			}
			int sanctionedMinutes = sanctionedMinutesFromFullDays + sanctionedMinutesFromPartialDays;

			return sanctionedMinutes;
		}

		public static DateTime CalculateRestTime (DateTime restStart, int restMinutes)
		{
			DateTime restEnd;

			int restingMinutesFirstDay = (int)(EndOfWorkday (restStart) - restStart).TotalMinutes;

			int remainingRest = restMinutes - restingMinutesFirstDay;

			if (remainingRest < 0) {
				restEnd = restStart.AddMinutes (restMinutes);
			} else {
				DateTime fullDaysRestStart = NextMorning (restStart);
				int days = (int)Math.Floor ((double)remainingRest / 600);
				DateTime afterFullRestDays = fullDaysRestStart.AddDays (days);
				int finalDaysRestMinutes = remainingRest - (days * 600);
				restEnd = afterFullRestDays.AddMinutes (finalDaysRestMinutes);
			}

			return restEnd;
		}
	}
}

