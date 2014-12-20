using System;
using Kaggle_HelpingSantasHelpers;
using NUnit.Framework;

namespace Kaggle_HelpingSantasHelpersTests
{
	[TestFixture]
	public class DateParserTests
	{
		[TestCase (1, "one,2014 11 2 4 5, two")]
		[TestCase (2, "three, 2014 11 2 4 5 , four")]
		[TestCase (3, "five,02014 11 02 0004 005, six")]
		[TestCase (4, "seven, 02014 11 02 0004 005 , eight")]
		public void ParseDateFromLine_VariousGoodInputs_ShouldReturnCorrectDateTimeObject (int caseNumber, string inputLine)
		{
			DateTime parsedDate = DateParser.ParseDateFromLine (inputLine);

			string whichCase = "@ case " + caseNumber.ToString ();

			Assert.AreEqual (2014, parsedDate.Year, "year" + whichCase);
			Assert.AreEqual (11, parsedDate.Month, "month" + whichCase);
			Assert.AreEqual (2, parsedDate.Day, "day" + whichCase);
			Assert.AreEqual (4, parsedDate.Hour, "hour" + whichCase);
			Assert.AreEqual (5, parsedDate.Minute, "minute" + whichCase);
		}
	}
}

