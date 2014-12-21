using System;
using Kaggle_HelpingSantasHelpers;
using NUnit.Framework;

namespace Kaggle_HelpingSantasHelpersTests
{
	[TestFixture]
	public class DateParserTests
	{
		[TestCase ("one,2014 11 2 4 5, two")]
		[TestCase ("three, 2014 11 2 4 5 , four")]
		[TestCase ("five,02014 11 02 0004 005, six")]
		[TestCase ("seven, 02014 11 02 0004 005 , eight")]
		public void ParseDateFromLine_VariousGoodInputs_ShouldReturnCorrectDateTimeObject (string inputLine)
		{
			DateTime parsedDate = DateParser.ParseDateFromLine (inputLine);

			Assert.AreEqual (2014, parsedDate.Year, "year" + TestContext.CurrentContext.Test.Name);
			Assert.AreEqual (11, parsedDate.Month, "month" + TestContext.CurrentContext.Test.Name);
			Assert.AreEqual (2, parsedDate.Day, "day" + TestContext.CurrentContext.Test.Name);
			Assert.AreEqual (4, parsedDate.Hour, "hour" + TestContext.CurrentContext.Test.Name);
			Assert.AreEqual (5, parsedDate.Minute, "minute" + TestContext.CurrentContext.Test.Name);
		}
	}
}

