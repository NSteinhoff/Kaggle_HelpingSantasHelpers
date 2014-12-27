using System;
using NUnit.Framework;
using Kaggle_HelpingSantasHelpers;

namespace Kaggle_HelpingSantasHelpersTests
{
	[TestFixture]
	public class HoursTests
	{
		[TestCase (1)]
		[TestCase (8)]
		[TestCase (10)]
		[TestCase (23)]
		public void StartOfWorkday_DifferentTimes_ReturnSameDayAt9 (int hour)
		{
			DateTime testTime = new DateTime (2014, 3, 11, hour, 25, 36);

			DateTime expectedDatetime = new DateTime (2014, 3, 11, 9, 0, 0);

			DateTime actualDatetime = Hours.StartOfWorkday (testTime);

			Assert.AreEqual (expectedDatetime, actualDatetime, TestContext.CurrentContext.Test.Name);
		}

		[TestCase (1)]
		[TestCase (8)]
		[TestCase (10)]
		[TestCase (23)]
		public void StartOfWorkday_DifferentTimes_ReturnSameDayAt19 (int hour)
		{
			DateTime testTime = new DateTime (2014, 3, 11, hour, 25, 36);

			DateTime expectedDatetime = new DateTime (2014, 3, 11, 19, 0, 0);

			DateTime actualDatetime = Hours.EndOfWorkday (testTime);

			Assert.AreEqual (expectedDatetime, actualDatetime, TestContext.CurrentContext.Test.Name);
		}

		[TestCase (1)]
		[TestCase (8)]
		[TestCase (10)]
		[TestCase (23)]
		public void NextMorning_DifferentTimes_ReturnNextMorningAt9 (int hour)
		{
			DateTime testTime = new DateTime (2014, 3, 11, hour, 25, 36);

			DateTime expectedDatetime = new DateTime (2014, 3, 12, 9, 0, 0);

			DateTime actualDatetime = Hours.NextMorning (testTime);

			Assert.AreEqual (expectedDatetime, actualDatetime, TestContext.CurrentContext.Test.Name);
		}

		[TestCase (1, 0, 600)]
		[TestCase (8, 59, 600)]
		[TestCase (9, 1, 599)]
		[TestCase (18, 1, 59)]
		[TestCase (18, 59, 1)]
		[TestCase (23, 0, 0)]
		public void MinutesLeftInWorkday_DifferentTimes_ReturnCorrectNumber (int hour, int minute, int expectedMinutes)
		{
			DateTime testTime = new DateTime (2014, 3, 11, hour, minute, 0);

			int actualMinutes = Hours.MinutesLeftInWorkday (testTime);

			Assert.AreEqual (expectedMinutes, actualMinutes, TestContext.CurrentContext.Test.Name);
		}

		[TestCase (1, 0, 11, 9, 0)]
		[TestCase (8, 59, 11, 9, 0)]
		[TestCase (9, 1, 11, 9, 1)]
		[TestCase (18, 1, 11, 18, 1)]
		[TestCase (18, 59, 11, 18, 59)]
		[TestCase (19, 0, 12, 9, 0)]
		[TestCase (23, 0, 12, 9, 0)]
		public void NextSanctionedMinute_DifferentTimes_ReturnCorrectTime (int hour, int minute, int expectedDay, int expectedHour, int expectedMinute)
		{
			DateTime testTime = new DateTime (2014, 3, 11, hour, minute, 0);

			DateTime expectedTime = new DateTime (2014, 3, expectedDay, expectedHour, expectedMinute, 0);

			DateTime actualTime = Hours.NextSanctionedMinute (testTime);

			Assert.AreEqual (expectedTime, actualTime, TestContext.CurrentContext.Test.Name);
		}

	}
}

