﻿using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Kaggle_HelpingSantasHelpers
{
	public static class ToyOrderBook
	{
		private const int MAX_ORDER_DURATION_MINUTES = 2400;
		public static List<List<ToyOrder>> orderLists;
		public static List<ToyOrder> fullDayOrders;
		public static List<ToyOrder> completedOrders;

		public static int orderBracketsCount { get; set; } = 2400;
		//240;

		public static double orderBracketsQuotient {
			get{ return (double)MAX_ORDER_DURATION_MINUTES / (double)orderBracketsCount; } 
		}

		public static ToyOrder lastOrderCompleted {
			get { 
				return completedOrders.MaxBy (x => x.finishTime);
			}
		}

		public static void SetupOrderLists ()
		{
			orderLists = new List<List<ToyOrder>> ();
			fullDayOrders = new List<ToyOrder> ();
			completedOrders = new List<ToyOrder> ();

			for (int i = 0; i < orderBracketsCount; i++) {
				orderLists.Add (new List<ToyOrder> ());
			}
			orderLists.Add (fullDayOrders);
		}

		public static int AddNewOrdersToOrderBook (List<string> incomingOrders)
		{
			int ordersAdded = 0;

			foreach (string order in incomingOrders) {
				ToyOrder newOrder = new ToyOrder (order);
				int listIndex = FilterNewOrderIntoBracket (newOrder);
				if (listIndex > -1) {
					ordersAdded++;
				}
			}
			return ordersAdded;
		}

		public static bool RemoveOrderFromBooks (ToyOrder order)
		{
			bool isOrderRemoved = false;

			int bracketNumber = CalculateBracketNumber (order);

			if (orderLists [bracketNumber].Count > 0) {
				isOrderRemoved = orderLists [bracketNumber].Remove (order);

			}
			return isOrderRemoved;
		}

		public static int CountAllOrdersInBook ()
		{
			int ordersPresent = 0;
			foreach (List<ToyOrder> list in ToyOrderBook.orderLists) {
				ordersPresent += list.Count;
			}
			return ordersPresent;
		}

		public static void PrintCompletedOrdersToConsole (int maxToPrint = 25)
		{
			if (completedOrders.Count <= maxToPrint) {
				foreach (ToyOrder toy in completedOrders) {
					Console.WriteLine (toy.ToString ());
				}
			} else {
				for (int i = 0; i < 10; i++) {
					ToyOrder toy = completedOrders [i];
					Console.WriteLine (toy.ToString ());
				}
				Console.WriteLine ("---");
				for (int j = completedOrders.Count - 1 - 10; j < completedOrders.Count; j++) {
					ToyOrder toy = completedOrders [j];
					Console.WriteLine (toy.ToString ());
				}

			}

		}

		private static int FilterNewOrderIntoBracket (ToyOrder toyOrder)
		{
			int listIndex = CalculateBracketNumber (toyOrder);

			orderLists [listIndex].Add (toyOrder);

			return listIndex;
		}

		private static int CalculateBracketNumber (ToyOrder toyOrder)
		{
			int bracketNumber = 0;

			if (toyOrder.durationMinutes < MAX_ORDER_DURATION_MINUTES) {
				bracketNumber = (int)Math.Floor ((double)toyOrder.durationMinutes / orderBracketsQuotient);
			} else {
				bracketNumber = ToyOrderBook.orderBracketsCount;
			}

			return bracketNumber;
		}
	}
}

