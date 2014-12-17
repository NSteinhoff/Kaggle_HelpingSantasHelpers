using System;

namespace Kaggle_HelpingSantasHelpers
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Start of program. Press any key....");
			Console.ReadKey ();

			DataReader.OpenReadStream (DataReader.filePath);

			ElfCommander.HireElves (10);

			// TODO transfer Testing() functionality to testing suite!
			DataReader.Testing ();

			DataReader.CloseReadStream ();

			Console.WriteLine ("End of program.");
		}
	}
}


