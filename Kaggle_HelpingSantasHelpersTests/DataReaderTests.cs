using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Kaggle_HelpingSantasHelpers;

namespace Kaggle_HelpingSantasHelpersTests
{
	[TestFixture]
	public class DataReaderTests
	{
		string dummyFileName;
		string dummyLineText;
		StreamReader reader;
		const int TEN_THOUSAND = 10000;

		[SetUp]
		public void Setup ()
		{
			dummyFileName = "dummyFile.txt";
			dummyLineText = "dummyText";

			SetupDummyFile (dummyFileName, dummyLineText, TEN_THOUSAND);

			reader = DataReader.OpenReadStream (dummyFileName);
		}

		[TearDown]
		public void Teardown ()
		{
			DataReader.CloseReadStream ();
			File.Delete (dummyFileName);
		}

		[Test]
		public void OpenReadStream_FileExists_ShouldReadHeader ()
		{
			string dummyLine = reader.ReadLine ();

			Assert.AreEqual ("header", dummyLine);
		}

		[Test]
		public void ReadLinesFromStream_TenThousandLines_ShouldReturnListOfTenThousandStrings ()
		{
			List<string> lines = DataReader.ReadLinesFromStream (TEN_THOUSAND);

			Assert.AreEqual (TEN_THOUSAND, lines.Count);
			Assert.AreEqual (dummyLineText + "_" + (TEN_THOUSAND - 1).ToString (), lines [TEN_THOUSAND - 1]);
		}

		private static void SetupDummyFile (string dummyFileName, string dummyLineText, int numberOfLines = 1)
		{
			FileStream dummyStream = File.Create (dummyFileName);
			StreamWriter dummyWriter = new StreamWriter (dummyStream);
			dummyWriter.WriteLine ("header");

			for (int i = 1; i < numberOfLines; i++) {
				dummyWriter.WriteLine (dummyLineText + "_" + i);
			}

			dummyWriter.Flush ();
			dummyWriter.Close ();
		}
	}
}

