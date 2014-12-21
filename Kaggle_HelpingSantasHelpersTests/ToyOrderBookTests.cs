using System;
using System.Collections.Generic;
using NUnit.Framework;
using Kaggle_HelpingSantasHelpers;

namespace Kaggle_HelpingSantasHelpersTests
{
	[TestFixture]
	public class ToyOrderBookTests
	{
		[Test]
		public void ToyOrder_240Brackets_ShouldCreate241Lists ()
		{
			ToyOrderBook.orderBracketsCount = 240;

			ToyOrderBook.SetupOrderLists ();
			int listCount = ToyOrderBook.orderLists.Count;

			Assert.AreEqual (241, listCount);
		}

		[Test]
		public void AddNewOrdersToOrderBook_SingleOrder_AddOneOrder ()
		{
			ToyOrderBook.SetupOrderLists ();
			string newOrderString = "2,2014 1 1 0 1,5";
			List<string> newOrderStrings = new List<string> ();
			newOrderStrings.Add (newOrderString);

			int ordersAdded = ToyOrderBook.AddNewOrdersToOrderBook (newOrderStrings);
			int ordersPresent = ToyOrderBook.CountAllOrdersInBook ();
				
			Assert.AreEqual (1, ordersAdded);
			Assert.AreEqual (1, ordersPresent);
		}

		[TestCase ("2,2014 1 1 0 1,0", 0, 240)]
		[TestCase ("2,2014 1 1 0 1,9", 0, 240)]
		[TestCase ("2,2014 1 1 0 1,10", 1, 240)]
		[TestCase ("2,2014 1 1 0 1,2399", 239, 240)]
		[TestCase ("2,2014 1 1 0 1,2400", 240, 240)]
		[TestCase ("2,2014 1 1 0 1,10000", 240, 240)]
		[TestCase ("2,2014 1 1 0 1,0", 0, 120)]
		[TestCase ("2,2014 1 1 0 1,19", 0, 120)]
		[TestCase ("2,2014 1 1 0 1,20", 1, 120)]
		[TestCase ("2,2014 1 1 0 1,2399", 119, 120)]
		[TestCase ("2,2014 1 1 0 1,2400", 120, 120)]
		[TestCase ("2,2014 1 1 0 1,10000", 120, 120)]
		public void AddNewOrdersToOrderBook_VariousBracketsWithVariousOrderLengths_AddToCorrectBracket (string newOrderString, int correctBracket, int numberOfBrackets)
		{
			ToyOrderBook.orderBracketsCount = numberOfBrackets;
			ToyOrderBook.SetupOrderLists ();
			List<string> newOrderStrings = new List<string> ();
			newOrderStrings.Add (newOrderString);

			ToyOrderBook.AddNewOrdersToOrderBook (newOrderStrings);
			int bracketOrderCount = ToyOrderBook.orderLists [correctBracket].Count;

			Assert.AreEqual (1, bracketOrderCount, TestContext.CurrentContext.Test.Name);
		}

		[TestCase (3, 2, 1)]
		[TestCase (1, 1, 0)]
		[TestCase (11, 2, 9)]
		public void RemoveOrder_SingleOrder_ShouldHaveNoOrderAfter (int toAdd, int toRemove, int resultingCount)
		{
			ToyOrderBook.SetupOrderLists ();
			string newOrderString = "2,2014 1 1 0 1,5";
			List<string> newOrderStrings = new List<string> ();

			for (int i = 0; i < toAdd; i++) {
				newOrderStrings.Add (newOrderString);
			}

			ToyOrderBook.AddNewOrdersToOrderBook (newOrderStrings);

			for (int j = 0; j < toRemove; j++) {
				ToyOrder toyOrder = ToyOrderBook.orderLists [0] [0];
				ToyOrderBook.RemoveOrderFromBooks (toyOrder);
			}

			int ordersPresent = ToyOrderBook.CountAllOrdersInBook ();
				
			Assert.AreEqual (resultingCount, ordersPresent);
		}
	}
}

