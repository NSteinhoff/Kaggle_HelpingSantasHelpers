﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Kaggle_HelpingSantasHelpers
{
	class MainClass
	{
		public const int N_ELVES = 900;
		public const int N_TOYS = 100000;

		public static void Main (string[] args)
		{
			Console.WriteLine ("Start of program. Press any key....");
			Console.ReadKey ();

			Stopwatch stopwatch = new Stopwatch ();
			stopwatch.Start ();


			try {
				ToyOrderBook.SetupOrderLists ();

				DataReader.OpenReadStream (DataReader.filePath);
				Console.WriteLine (DataReader.ReadHeaderLine ());
				List<string> incomingOrders = DataReader.ReadLinesFromStream (N_TOYS);
				ToyOrderBook.AddNewOrdersToOrderBook (incomingOrders);
				DataReader.CloseReadStream ();

				ElfCommander.HireElves (N_ELVES);

				int remainingOrders = ToyOrderBook.CountAllOrdersInBook ();
				int processedOrders = 1;
				while (remainingOrders > 0) {
					if (processedOrders % 1000 == 0) {
						Console.WriteLine (String.Format ("Toys processed: {0}  {1}", processedOrders.ToString (), CalculateFractionComplete ().ToString ("F2")));
					}
					processedOrders++;

					Elf elf = ElfCommander.PickNextElf ();
					ToyOrder toy = elf.ChooseToy ();
					elf.BuildToy (toy);

					remainingOrders = ToyOrderBook.CountAllOrdersInBook ();
				}

				ToyOrderBook.PrintCompletedOrdersToConsole ();
				Console.WriteLine (String.Format ("Completed Toys: {0}", ToyOrderBook.completedOrders.Count));
				ToyOrder lastToy = ToyOrderBook.lastOrderCompleted;
				Console.WriteLine (String.Format ("Last Toy Completed: Toy {0} completed at {1} {2}", lastToy.iD, lastToy.finishTime.ToShortDateString (), lastToy.finishTime.ToShortTimeString ()));
				Console.WriteLine (String.Format ("Total Minutes: {0}", CalculateTotalMinutes ()));
				Console.WriteLine (String.Format ("Score: {0}", CalculateScore ()));

				WriteResultsFile ();

			} catch (Exception ex) {
				Console.WriteLine ("In 'Main()'");
				Console.WriteLine (ex.Message);
				Console.WriteLine (ex.ToString ());
			}

			stopwatch.Stop ();
			Console.WriteLine (String.Format ("Elapsed Time: {0}", stopwatch.Elapsed.ToString ()));
			Console.WriteLine ("End of program.");
		}

		private static double CalculateScore ()
		{
			return CalculateTotalMinutes () * Math.Log (1 + N_ELVES);
		}

		private static int CalculateTotalMinutes ()
		{
			return (int)(ToyOrderBook.lastOrderCompleted.finishTime - new DateTime (2014, 1, 1, 0, 0, 0)).TotalMinutes;
		}

		private static void WriteResultsFile ()
		{
			FileStream resultsFile = File.Create (String.Format ("/Users/nikosteinhoff/Desktop/santasHelpersResults_{0}.txt", DateTime.Now.Ticks.ToString ()));
			StreamWriter resultsWriter = new StreamWriter (resultsFile);
			resultsWriter.WriteLine ("ToyId,ElfId,StartTime,Duration");

			foreach (ToyOrder toy in ToyOrderBook.completedOrders) {
				resultsWriter.WriteLine (toy.ToString ());
			}

			resultsWriter.Flush ();
			resultsWriter.Close ();
		}

		public static double CalculateFractionComplete ()
		{
			double completedOrders = ToyOrderBook.completedOrders.Count;

			double fraction = completedOrders / N_TOYS;

			return fraction;
		}

	}
}


