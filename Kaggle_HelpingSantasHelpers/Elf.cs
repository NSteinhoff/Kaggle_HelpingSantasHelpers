using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaggle_HelpingSantasHelpers
{
	public class Elf
	{
		private int _id;
		private double _productivity;
		private DateTime _workingTill;

		public Elf (int id)
		{
			_id = id;
			_productivity = 1;
			_workingTill = new DateTime (2014, 1, 1, 0, 0, 0);
		}

		public int ID {
			get{ return _id; }
		}

		public double Productivity {
			get{ return _productivity; }
		}

		public DateTime NextAvailable {
			get { 
				if (_workingTill.Hour < 9) {
					return new DateTime (_workingTill.Year, _workingTill.Month, _workingTill.Day, 9, 0, 0);

				} else if (_workingTill.Hour > 19) {
					return new DateTime (_workingTill.Year, _workingTill.Month, _workingTill.Day + 1, 9, 0, 0); 
					
				} else {
					return _workingTill;
				}
			}
		}

		public DateTime EndOfDay {
			get{ return new DateTime (NextAvailable.Year, NextAvailable.Month, NextAvailable.Day, 19, 0, 0); }
		}

		public TimeSpan WorkTimeLeft {
			get { return EndOfDay - NextAvailable; }
		}

		public TimeSpan EffectiveWorkTimeLeft {
			get{ return new TimeSpan ((long)((double)WorkTimeLeft.Ticks * Productivity)); }
		}

		public DateTime EffectiveNextAvailable {
			get{ return NextAvailable - EffectiveWorkTimeLeft; }
		}

		public ToyOrder PickToy ()
		{
			ToyOrder nextToy = null;

			if (Productivity == 4 && NextAvailable.Hour == 9 && NextAvailable.Minute == 0 && ToyOrderBook.fullDayOrders.Count > 0) {
				nextToy = ToyOrderBook.fullDayOrders [0];

			} else {
				foreach (ToyOrder toy in ToyOrderBook.partOfDayOrders) {
					DateTime possibleStartTime = (NextAvailable > toy.ArrivalTime) ? NextAvailable : toy.ArrivalTime;

					DateTime possibleCompletionTime = possibleStartTime.AddMinutes (toy.DurationMinutes / Productivity);

					if (possibleCompletionTime < EndOfDay) {
						nextToy = toy;
						break;
					} 
				}
			
				if (nextToy == null) {
					_workingTill = new DateTime (NextAvailable.Year, NextAvailable.Month, NextAvailable.Day + 1, 9, 0, 0);
				} else {
					ToyOrderBook.RemoveOrderFromBooks (nextToy);

					// Calculate time and productivity adjustment
				}

			}

			return nextToy;
		}
	}
}

