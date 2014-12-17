using System;
using NUnit.Framework;
using Kaggle_HelpingSantasHelpers;

namespace Kaggle_HelpingSantasHelpersTests
{
	[TestFixture ()]
	public class DataReaderTest
	{
		[Test ()]
		public void OpenReadStreamTest ()
		{
			Assert.IsTrue (DataReader.OpenReadStream ());
			DataReader.CloseReadStream ();
		}

	}
}

