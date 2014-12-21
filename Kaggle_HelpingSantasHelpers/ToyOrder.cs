using System;

namespace Kaggle_HelpingSantasHelpers
{
	public class ToyOrder
	{
		private int _id;
		private DateTime _arrivalTime;
		private int _durationMinutes;

		private const int SANCTIONED_HOURS = 10;
		private const double FACTOR_SANCTIONED = 1.02;
		private const double FACTOR_UNSANCTIONED = 0.9;

		public ToyOrder (string orderString)
		{
			string[] orderComponents = orderString.Split (',');
			this._id = Convert.ToInt32 (orderComponents [0]);
			this._arrivalTime = DateParser.ParseDateFromLine (orderString);
			this._durationMinutes = Convert.ToInt32 (orderComponents [2]);
			this.elfId = 0;
		}

		public int iD {
			get { return this._id; }
		}

		public DateTime arrivalTime {
			get { return this._arrivalTime; }
		}

		public int durationMinutes {
			get { return this._durationMinutes; }
		}

		public int elfId{ get; set; }

		public bool exceedWorkingDay {
			get {
				return ((this.durationMinutes / 4) > (10 * 60));
			}
		}

		public static double maxProductivityNeutralMinutes {
			get {
				double maxUnsanctioned = -SANCTIONED_HOURS * Math.Log (FACTOR_SANCTIONED) / Math.Log (FACTOR_UNSANCTIONED);

				return (SANCTIONED_HOURS + maxUnsanctioned) * 60;
			}
		}
	}
}

