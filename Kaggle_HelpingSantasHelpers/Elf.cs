using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaggle_HelpingSantasHelpers
{
	public class Elf
	{
		private int _id;
		private double _productivity;
		private int _neededRest;

		public const double FACTOR_SANCTIONED = 1.02;
		public const double FACTOR_UNSANCTIONED = 0.9;

		public Elf (int id)
		{
			_id = id;
			_productivity = 1;
		}

		public int id {
			get{ return this._id; }
		}

		public double productivity {
			get{ return this._productivity; }
			private set { 
				double updatedValue; 
				if (value > 4) {
					updatedValue = 4;
				} else if (value < 0.25) {
					updatedValue = 0.25;
				} else {
					updatedValue = value;
				}
				this._productivity = updatedValue; 
			}
		}

		private DateTime workingTill { get; set; } = new DateTime (2014, 1, 1, 0, 0, 0);

		private int neededRest {
			get{ return this._neededRest; }
			set{ this._neededRest = value; }
		}

		public DateTime nextAvailable {
			get { 
				DateTime nextAvailable = Hours.NextSanctionedMinute (this.workingTill);

				if (this.neededRest == 0) {
					return nextAvailable;
				} else {
					return Hours.CalculateRestTime (nextAvailable, this.neededRest);
				}
			}
		}

		private int workTimeLeft {
			get { return Hours.MinutesLeftInWorkday (nextAvailable); }
		}

		private int effectiveWorkTimeLeft {
			get{ return (int)Math.Floor (workTimeLeft * productivity); }
		}

		private DateTime endOfSanctionedWorkday {
			get{ return new DateTime (nextAvailable.Year, nextAvailable.Month, nextAvailable.Day, 19, 0, 0); }
		}

		public ToyOrder ChooseToy ()
		{
			ToyOrder toy = null;

			DateTime nextAvailable = Hours.Duplicate (this.nextAvailable);

			double highSkilledCutoff = 3.8;
			bool isProductivityCappedElfAtStartOfDay = this.productivity >= highSkilledCutoff && nextAvailable.Hour <= 12;

			if (isProductivityCappedElfAtStartOfDay) {
				toy = PickLargestAvailableToy (nextAvailable);
			} else if (this.productivity < highSkilledCutoff) {
				toy = PickSmallestAvailableToy (nextAvailable);
			} else {
				toy = PickLargestCompletableToday (nextAvailable);
			}
				
			if (toy == null) {
				toy = PickErliestAvailableToy ();
			}

			return toy;
		}

		private ToyOrder PickLargestAvailableToy (DateTime nextAvailable)
		{
			ToyOrder toy = null;

			for (int i = ToyOrderBook.orderLists.Count - 1; i >= 0; i--) {
				List<ToyOrder> bracket = ToyOrderBook.orderLists [i];

				if (bracket.Count > 0) {
					toy = PickAvailableToyInBracket (bracket, nextAvailable);
				}

				if (toy != null) {
					break;
				}
			}

			return toy;
		}

		private ToyOrder PickSmallestAvailableToy (DateTime nextAvailable)
		{
			ToyOrder toy = null;

			for (int i = 0; i < ToyOrderBook.orderLists.Count; i++) {
				List<ToyOrder> bracket = ToyOrderBook.orderLists [i];

				if (bracket.Count > 0) {
					toy = PickAvailableToyInBracket (bracket, nextAvailable);
				}

				if (toy != null) {
					break;
				}
			}

			return toy;
		}

		private ToyOrder PickLargestCompletableToday (DateTime nextAvailable)
		{
			int bestBracket = (int)Math.Floor ((double)effectiveWorkTimeLeft / ToyOrderBook.orderBracketsQuotient);

			ToyOrder toy = null;

			for (int i = bestBracket; i >= 0; i--) {
				List<ToyOrder> bracket = ToyOrderBook.orderLists [i];

				if (bracket.Count > 0) {
					toy = PickAvailableToyInBracket (bracket, nextAvailable);
				}

				if (toy != null) {
					break;
				}
			}

			if (toy == null) {
				toy = PickSmallestAvailableToy (nextAvailable);
			}

			return toy;
		}

		private ToyOrder PickAvailableToyInBracket (List<ToyOrder> bracket, DateTime nextAvailable)
		{
			ToyOrder toy = null;
			ToyOrder firstToyInBracket = bracket [0];
			if (firstToyInBracket.arrivalTime <= nextAvailable) {
				toy = firstToyInBracket;
			}
			return toy;
		}

		private ToyOrder PickErliestAvailableToy ()
		{
			ToyOrder toy = null;
			List<ToyOrder> earliestToysInSubDayBrackets = new List<ToyOrder> ();
			List<ToyOrder> earliestToysInSuperDayBrackets = new List<ToyOrder> ();

			for (int i = 0; i <= Math.Min (effectiveWorkTimeLeft, ToyOrderBook.orderBracketsCount - 1); i++) {
				List<ToyOrder> bracket = ToyOrderBook.orderLists [i];
				if (bracket.Count > 0) {
					earliestToysInSubDayBrackets.Add (bracket [0]);
				}

			}

			for (int i = Math.Min (effectiveWorkTimeLeft + 1, ToyOrderBook.orderBracketsCount); i < ToyOrderBook.orderLists.Count; i++) {
				List<ToyOrder> bracket = ToyOrderBook.orderLists [i];
				if (bracket.Count > 0) {
					earliestToysInSuperDayBrackets.Add (bracket [0]);
				}
			}
				
			ToyOrder earliestSubDayToy = null;
			if (earliestToysInSubDayBrackets.Count > 0) {
				earliestSubDayToy = earliestToysInSubDayBrackets.OrderBy (x => x.arrivalTime).ToList () [0];
			}
			ToyOrder earliestSuperDayToy = null;
			if (earliestToysInSuperDayBrackets.Count > 0) {
				earliestSuperDayToy = earliestToysInSuperDayBrackets.OrderBy (x => x.arrivalTime).ToList () [0];
			}

			if (earliestSubDayToy == null) {
				toy = earliestSuperDayToy;
			} else if (earliestSuperDayToy == null) {
				toy = earliestSubDayToy;
			} else if ((earliestSubDayToy.arrivalTime - earliestSuperDayToy.arrivalTime).TotalDays > 1) {
				toy = earliestSuperDayToy;
			} else {
				toy = earliestSubDayToy;
			}

			return toy;
		}

		public void BuildToy (ToyOrder toy)
		{
			int minutesTillFinished = (int)Math.Ceiling ((double)toy.durationMinutes / this.productivity);

			DateTime startTime = CalculateStartTime (toy, minutesTillFinished);

			this.workingTill = startTime.AddMinutes (minutesTillFinished);

			int sanctionedMinutes = Hours.CalculateSanctionedMinutes (startTime, minutesTillFinished);
			int unsanctionedMinutes = minutesTillFinished - sanctionedMinutes;

			this.neededRest = unsanctionedMinutes;

			toy.elfId = this.id;
			toy.startTime = startTime;
			toy.finalDuration = minutesTillFinished;

			UpdateProductivity (sanctionedMinutes, unsanctionedMinutes);

			ToyOrderBook.completedOrders.Add (toy);
			ToyOrderBook.RemoveOrderFromBooks (toy);
		}

		DateTime CalculateStartTime (ToyOrder toy, int minutesTillFinished)
		{
			DateTime startTime;
			DateTime earliestAvailableTime = new DateTime (Math.Max (toy.arrivalTime.Ticks, this.nextAvailable.Ticks));
			startTime = Hours.NextSanctionedMinute (earliestAvailableTime);

			bool isLessThanFullDayToy = minutesTillFinished < 600;

			int minutesLeftInDay = Hours.MinutesLeftInWorkday (startTime);
			double overtimeFractionForbidden = 1 - Math.Min (Math.Pow (MainClass.CalculateFractionComplete (), 5), 0.15);
			bool isOutsideOvertimeTolerance = (minutesTillFinished * overtimeFractionForbidden) > minutesLeftInDay;

			if (isLessThanFullDayToy && isOutsideOvertimeTolerance) {
				startTime = Hours.NextMorning (startTime);
			} 
			return startTime;
		}

		private void UpdateProductivity (int sanctionedMinutes, int unsanctionedMinutes)
		{
			decimal sanctionedHours = (decimal)sanctionedMinutes / 60;
			decimal unsanctionedHours = (decimal)unsanctionedMinutes / 60;

			double productivityFactor = Math.Pow (FACTOR_SANCTIONED, (double)sanctionedHours) * Math.Pow (FACTOR_UNSANCTIONED, (double)unsanctionedHours);

			this.productivity = this.productivity * productivityFactor;
		}

	}
}

