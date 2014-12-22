using NUnit.Framework;
using System;
using System.Collections.Generic;
using Kaggle_HelpingSantasHelpers;



namespace Kaggle_HelpingSantasHelpersTests
{
	[TestFixture]
	public class ElfTests
	{
		[TestCase ("2, 2014 1 1 0 0, 5", 2014, 1, 1, 9, 5)]
		[TestCase ("2, 2014 1 1 0 0, 610", 2014, 1, 2, 9, 10)]
		[TestCase ("2, 2014 1 1 18 59, 610", 2014, 1, 3, 9, 9)]
		[TestCase ("2, 2014 1 2 19 00, 10", 2014, 1, 3, 9, 10)]
		[TestCase ("2, 2014 1 2 18 55, 10", 2014, 1, 3, 9, 5)]
		public void NextAvailable_AvailableToyOrders_ReturnsCorrectTime (string toyOrderString, int year, int month, int day, int hour, int minute)
		{
			ToyOrder toy = new ToyOrder (toyOrderString);

			Elf elf = new Elf (1);
			DateTime shouldFinish = new DateTime (year, month, day, hour, minute, 0);

			elf.BuildToy (toy);

			Assert.AreEqual (shouldFinish, elf.nextAvailable, TestContext.CurrentContext.Test.Name);
		}

		[TestCase ("2, 2014 1 1 0 0, 60", 1, 0)]
		[TestCase ("2, 2014 1 1 18 0, 120", 1, 1)]
		[TestCase ("2, 2014 1 1 9 0, 1500", 11, 14)]
		public void Productivity_VariousOrders_UpdatesProductivity (string toyOrderString, double sanctionedHours, double unsanctionedHours)
		{
			ToyOrder toy = new ToyOrder (toyOrderString);

			Elf elf = new Elf (1);

			elf.BuildToy (toy);

			double expectedProductivity = 1 * Math.Pow (Elf.FACTOR_SANCTIONED, sanctionedHours) * Math.Pow (Elf.FACTOR_UNSANCTIONED, unsanctionedHours);

			Assert.AreEqual (expectedProductivity, elf.productivity);
		}

		[Test]
		public void ChooseToy_ToysWithDifferentDurationsAndStartTimes_PickQuickestAvailableToy ()
		{
			List<string> orders = new List<string> ();
			orders.Add ("1, 2014 1 1 8 0, 20");
			orders.Add ("2, 2014 1 1 7 0, 40");
			orders.Add ("3, 2014 1 1 9 0, 60");
			orders.Add ("4, 2014 1 1 12 0, 10");
			ToyOrderBook.SetupOrderLists ();
			ToyOrderBook.AddNewOrdersToOrderBook (orders);

			ToyOrder shouldChooseToy = new ToyOrder ("1, 2014 1 1 0 0, 20");

			Elf elf = new Elf (1);

			ToyOrder chosenToy = elf.ChooseToy ();

			Assert.AreEqual (shouldChooseToy.iD, chosenToy.iD);
		}

		[Test]
		public void ChooseToy_ToysWithDifferentDurationsAndStartTimes_PickEarliestToy ()
		{
			List<string> orders = new List<string> ();
			orders.Add ("1, 2014 1 2 8 0, 20");
			orders.Add ("2, 2014 1 3 7 0, 40");
			orders.Add ("3, 2014 1 2 9 0, 60");
			orders.Add ("4, 2014 1 1 12 0, 10");
			ToyOrderBook.SetupOrderLists ();
			ToyOrderBook.AddNewOrdersToOrderBook (orders);

			ToyOrder shouldChooseToy = new ToyOrder ("4, 2014 1 1 12 0, 10");

			Elf elf = new Elf (1);

			ToyOrder chosenToy = elf.ChooseToy ();

			Assert.AreEqual (shouldChooseToy.iD, chosenToy.iD);
		}
	}
}

