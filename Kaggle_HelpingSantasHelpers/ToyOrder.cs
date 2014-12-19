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

		public bool ExceedWorkingDayProductivityNeutrality {
			get {
				return ((DurationMinutes / 4) > MaxProductivityNeutralMinutes);
			}
		}

		public static double MaxProductivityNeutralMinutes {
			get {
				int sanctionedHours = 10;
				double factorSanctioned = 1.02;
				double factorUnsanctioned = 0.9;

				double maxUnsanctioned = -sanctionedHours * Math.Log (factorSanctioned) / Math.Log (factorUnsanctioned);

				return (sanctionedHours + maxUnsanctioned) * 60;
			}
		}

	}
}

