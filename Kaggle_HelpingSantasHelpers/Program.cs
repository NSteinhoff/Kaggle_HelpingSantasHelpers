using System;

namespace Kaggle_HelpingSantasHelpers
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Start of program. Press any key....");
			Console.ReadKey ();


			try {



				DataReader.OpenReadStream (DataReader.filePath);
				Console.WriteLine (DataReader.ReadHeaderLine ());

				ElfCommander.HireElves (10);

				DataReader.CloseReadStream ();

			} catch (Exception ex) {
				Console.WriteLine ("In 'Main()'");
				Console.WriteLine (ex.Message);
				Console.WriteLine (ex.ToString ());
			}

			Console.WriteLine ("End of program.");
		}
	}
}


