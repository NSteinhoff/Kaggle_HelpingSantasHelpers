using System;
using System.Collections.Generic;

namespace Kaggle_HelpingSantasHelpers
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Start of program. Press any key....");
			Console.ReadKey ();


			try {
				ToyOrderBook.SetupOrderLists ();
				DataReader.OpenReadStream (DataReader.filePath);
				Console.WriteLine (DataReader.ReadHeaderLine ());
				List<string> incomingOrders = DataReader.ReadLinesFromStream (10000);
				ToyOrderBook.AddNewOrdersToOrderBook (incomingOrders);
				ElfCommander.HireElves (10);

				int remainingOrders = 0; 

				while (remainingOrders > 0) {
					remainingOrders = ToyOrderBook.CountAllOrdersInBook ();





				}

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


