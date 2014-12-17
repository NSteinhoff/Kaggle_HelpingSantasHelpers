using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaggle_HelpingSantasHelpers
{
	public static class ToyOrderBook
	{
		public static List<ToyOrder> partOfDayOrders = new List<ToyOrder> ();
		public static List<ToyOrder> fullDayOrders = new List<ToyOrder> ();

		public static int AddNewOrdersToOrderBook (List<string> incomingOrders)
		{
			int ordersAdded = 0;

			foreach (string order in incomingOrders) {

				ToyOrder newOrder = new ToyOrder (order);

				if (newOrder.ExceedWorkingDay) {
					fullDayOrders.Add (newOrder);
				} else {
					partOfDayOrders.Add (newOrder);
				}
				ordersAdded++;
			}

			return ordersAdded;
		}

		public static bool RemoveOrderFromBooks (ToyOrder order)
		{
			bool isOrderRemoved = false;

			if (!isOrderRemoved)
				isOrderRemoved = partOfDayOrders.Remove (order);

			if (!isOrderRemoved)
				isOrderRemoved = fullDayOrders.Remove (order);

			return isOrderRemoved;
		}
	}
}

