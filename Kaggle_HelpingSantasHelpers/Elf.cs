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

		public int id {
			get{ return _id; }
		}

		public double productivity {
			get{ return _productivity; }
		}

		public DateTime nextAvailable {
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

		private TimeSpan workTimeLeft {
			get { return endOfSanctionedWorkday - nextAvailable; }
		}

		private TimeSpan effectiveWorkTimeLeft {
			get{ return new TimeSpan ((long)((double)workTimeLeft.Ticks * productivity)); }
		}

		private DateTime endOfSanctionedWorkday {
			get{ return new DateTime (nextAvailable.Year, nextAvailable.Month, nextAvailable.Day, 19, 0, 0); }
		}


	}
}

