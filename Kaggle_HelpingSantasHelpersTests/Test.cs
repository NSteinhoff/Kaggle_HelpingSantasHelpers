using NUnit.Framework;
using System;
using Kaggle_HelpingSantasHelpers;



namespace Kaggle_HelpingSantasHelpersTests
{
	[TestFixture ()]
	public class TestDummy
	{
		[Test ()]
		public void TestCaseNotEqual ()
		{
			Assert.AreNotEqual (1, 0);
		}

		[Test ()]
		public void TestCaseEqual ()
		{
			Assert.AreEqual (1, 1);
		}
	}
}

