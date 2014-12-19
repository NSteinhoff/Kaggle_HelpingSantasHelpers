using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Kaggle_HelpingSantasHelpers;

namespace Kaggle_HelpingSantasHelpersTests
{
	[TestFixture ()]
	public class DataReaderTests
	{
		[Test ()]
		public void OpenReadStream_FileExists_ShouldReadHeader ()
		{
			string dummyFileName = "dummyFile.txt";
			string dummyLineText = "dummyText";

			SetupDummyFile (dummyFileName, dummyLineText);

			StreamReader reader = DataReader.OpenReadStream (dummyFileName);
			string dummyLine = reader.ReadLine ();

			Assert.AreEqual ("header", dummyLine);

			reader.Close ();
			File.Delete (dummyFileName);
		}

		[Test ()]
		public void OpenReadStream_FileDoesNotExist_ShouldThrowFileNotFoundException ()
		{
			string dummyFileName = "dummyFileThatDoesNotExist.txt";

			Assert.Throws<FileNotFoundException> (() => DataReader.OpenReadStream (dummyFileName));
		}

		[Test ()]
		public void ReadLinesFromStream_TenThousandLines_ShouldReturnListOfTenThousandStrings ()
		{
			string dummyFileName = "dummyFileTenThousandLines.txt";
			string dummyLineText = "dummyText";
			const int TEN_THOUSAND = 10000;
			SetupDummyFile (dummyFileName, dummyLineText, TEN_THOUSAND);

			DataReader.OpenReadStream (dummyFileName);
			List<string> lines = DataReader.ReadLinesFromStream (TEN_THOUSAND);

			Assert.AreEqual (TEN_THOUSAND, lines.Count);
			Assert.AreEqual (dummyLineText + "_" + (TEN_THOUSAND - 1).ToString (), lines [TEN_THOUSAND - 1]);

			DataReader.CloseReadStream ();
			File.Delete (dummyFileName);
		}

		[Test ()]
		public void ReadNewIncomingOrdersFromStream_MoreDaysThanNeededInFile_ShouldReturnOnlyCorrectSubset ()
		{


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

