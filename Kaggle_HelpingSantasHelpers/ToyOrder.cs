using System;

namespace Kaggle_HelpingSantasHelpers
{
	public class ToyOrder
	{
		private int _id;
		private DateTime _arrivalTime;
		private int _durationMinutes;

		public ToyOrder (int id, DateTime arrivalTime, int durationMinutes)
		{
			_id = id;
			_arrivalTime = arrivalTime;
			_durationMinutes = durationMinutes;
		}

		public ToyOrder (string orderString)
		{
			string[] orderComponents = orderString.Split (',');


			_id = Convert.ToInt32 (orderComponents [0]);


			string[] dateComponents = orderComponents [1].Split (' ');

			int year = Convert.ToInt32 (dateComponents [0]);
			int month = Convert.ToInt32 (dateComponents [1]);
			int day = Convert.ToInt32 (dateComponents [2]);
			int hour = Convert.ToInt32 (dateComponents [3]);
			int minute = Convert.ToInt32 (dateComponents [4]);
			int second = 0;

			_arrivalTime = new DateTime (year, month, day, hour, minute, second);


			_durationMinutes = Convert.ToInt32 (orderComponents [2]);
		}

		public void Print ()
		{
			Console.WriteLine (String.Format ("Toy {0}, Arrival {1}, Duration {2}", this._id, this._arrivalTime, this._durationMinutes));
		}

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

