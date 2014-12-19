using System;

namespace Kaggle_HelpingSantasHelpers
{
	public class ToyOrder
	{
		private int _id;
		private DateTime _arrivalTime;
		private int _durationMinutes;

		public ToyOrder (string orderString)
		{
			string[] orderComponents = orderString.Split (',');

			_id = Convert.ToInt32 (orderComponents [0]);

			_arrivalTime = DateParser.ParseDateFromLine (orderString);

			_durationMinutes = Convert.ToInt32 (orderComponents [2]);
		}

		//		public void Print ()
		//		{
		//			Console.WriteLine (String.Format ("Toy {0}, Arrival {1}, Duration {2}", this._id, this._arrivalTime, this._durationMinutes));
		//		}

		public int ID {
			get { return _id; }
		}

		public DateTime ArrivalTime {
			get { return _arrivalTime; }
		}

		public int DurationMinutes {
			get { return _durationMinutes; }
		}

		public bool ExceedWorkingDay {
			get { return ((DurationMinutes / 4) > (10 * 60)); }
		}
	}
}

