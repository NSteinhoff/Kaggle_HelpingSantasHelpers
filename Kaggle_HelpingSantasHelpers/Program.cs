using System;

namespace Kaggle_HelpingSantasHelpers
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Start of program.");

			DataReader.OpenReadStream (@"/Users/nikosteinhoff/Data/Kaggle_HelpingSantasHelpers/toys_rev2.csv");

			ElfCommander.HireElves (10);


			// TODO 
			DataReader.Testing ();

			DataReader.CloseReadStream ();

			Console.WriteLine ("End of program.");
		}
			
	
	}
}


