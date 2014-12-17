using NUnit.Framework;
using System;
using Kaggle_HelpingSantasHelpers;



namespace Kaggle_HelpingSantasHelpersTests
{
	[TestFixture ()]
	public class TestDummy
	{
		[Test ()]
		public void TestCaseFail ()
		{
			Assert.AreEqual (1, 0);
		}

		[Test ()]
		public void TestCasePass ()
		{
			Assert.AreEqual (1, 1);
		}
	}
}

