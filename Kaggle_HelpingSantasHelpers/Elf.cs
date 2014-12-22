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
				DateTime nextAvailable;
				if (this.workingTill.Hour < 9) {
					nextAvailable = new DateTime (this.workingTill.Year, this.workingTill.Month, this.workingTill.Day, 9, 0, 0);

				} else if (this.workingTill.Hour >= 19) {
					nextAvailable = (new DateTime (this.workingTill.Year, this.workingTill.Month, this.workingTill.Day, 9, 0, 0)).AddDays (1); 
					
				} else {
					nextAvailable = this.workingTill;
				}

				return nextAvailable + CalculateRestTime (this.neededRest);
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

		public ToyOrder ChooseToy ()
		{
			ToyOrder toy = null;

			if (this.productivity == 4) {
				toy = PickLargestAvailableToy ();
			} else {
				toy = PickSmallestAvailableToy ();
			}
			if (toy == null) {
				toy = PickErliestAvailableToy ();
			}

			return toy;
		}

		private ToyOrder PickLargestAvailableToy ()
		{
			ToyOrder toy = null;

			for (int i = ToyOrderBook.orderLists.Count - 1; i >= 0; i--) {
				List<ToyOrder> bracket = ToyOrderBook.orderLists [i];

				if (bracket.Count > 0) {
					toy = PickAvailableToyInBracket (bracket);
				}

				if (toy != null) {
					break;
				}
			}

			return toy;
		}

		private ToyOrder PickSmallestAvailableToy ()
		{
			ToyOrder toy = null;

			for (int i = 0; i < ToyOrderBook.orderLists.Count; i++) {
				List<ToyOrder> bracket = ToyOrderBook.orderLists [i];

				if (bracket.Count > 0) {
					toy = PickAvailableToyInBracket (bracket);
				}

				if (toy != null) {
					break;
				}
			}

			return toy;
		}

		private ToyOrder PickAvailableToyInBracket (List<ToyOrder> bracket)
		{
			ToyOrder toy = null;
			ToyOrder firstToyInBracket = bracket [0];
			if (firstToyInBracket.arrivalTime <= this.nextAvailable) {
				toy = firstToyInBracket;
			}
			return toy;
		}

		private ToyOrder PickErliestAvailableToy ()
		{
			ToyOrder toy = null;
			List<ToyOrder> earliestToysInBrackets = new List<ToyOrder> ();

			for (int i = 0; i < ToyOrderBook.orderLists.Count; i++) {
				List<ToyOrder> bracket = ToyOrderBook.orderLists [i];
				if (bracket.Count > 0) {
					earliestToysInBrackets.Add (bracket [0]);
				}
			}
				
			toy = earliestToysInBrackets.OrderBy (x => x.arrivalTime).ToList () [0];

			return toy;
		}


		public void BuildToy (ToyOrder toy)
		{
			DateTime startTime;
			DateTime earliestAvailableTime = new DateTime (Math.Max (toy.arrivalTime.Ticks, this.nextAvailable.Ticks));
			if (earliestAvailableTime.Hour < 9) {
				startTime = new DateTime (earliestAvailableTime.Year, earliestAvailableTime.Month, earliestAvailableTime.Day, 9, 0, 0);

			} else if (earliestAvailableTime.Hour >= 19) {
				startTime = (new DateTime (earliestAvailableTime.Year, earliestAvailableTime.Month, earliestAvailableTime.Day, 9, 0, 0)).AddDays (1); 

			} else {
				startTime = earliestAvailableTime;
			}

			int minutesTillFinished = (int)Math.Ceiling ((double)toy.durationMinutes / this.productivity);

			DateTime endOfStartDay = new DateTime (startTime.Year, startTime.Month, startTime.Day, 19, 0, 0);
			int minutesLeftInDay = (int)(endOfStartDay - startTime).TotalMinutes;

			double overtimeFractionAllowed = MainClass.CalculateFractionComplete ();

			if ((minutesTillFinished < 600) && ((minutesTillFinished * overtimeFractionAllowed) > minutesLeftInDay)) {
				DateTime tempTime = new DateTime (startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, startTime.Second);
				startTime = (new DateTime (tempTime.Year, tempTime.Month, tempTime.Day, 9, 0, 0)).AddDays (1); 
			}


			this.workingTill = startTime.AddMinutes (minutesTillFinished);

			int sanctionedMinutes = CalculateSanctionedMinutes (startTime, minutesTillFinished);
			int unsanctionedMinutes = minutesTillFinished - sanctionedMinutes;

			this.neededRest = unsanctionedMinutes;


			toy.elfId = this.id;
			toy.startTime = startTime;
			toy.finalDuration = minutesTillFinished;

			UpdateProductivity (sanctionedMinutes, unsanctionedMinutes);

			ToyOrderBook.completedOrders.Add (toy);
			ToyOrderBook.RemoveOrderFromBooks (toy);
		}

		private int CalculateSanctionedMinutes (DateTime start, int duration)
		{
			DateTime end = start.AddMinutes (duration);

			DateTime endOfStartDay = new DateTime (start.Year, start.Month, start.Day, 19, 0, 0);
			int sanctionedMinutesStartDay = (int)(endOfStartDay - start).TotalMinutes;
			int fullDays = (int)Math.Floor ((end - start).TotalDays);
			int restMinutes = (int)(end - start.AddDays (fullDays)).TotalMinutes;

			int sanctionedMinutesFromFullDays = 10 * 60 * fullDays;
			int sanctionedMinutesFromPartialDays = 0;

			if (restMinutes <= sanctionedMinutesStartDay) {
				sanctionedMinutesFromPartialDays = restMinutes;
			} else if (restMinutes <= (sanctionedMinutesStartDay + 14 * 60)) {
				sanctionedMinutesFromPartialDays = sanctionedMinutesStartDay;
			} else {
				sanctionedMinutesFromPartialDays = restMinutes - (14 * 60);
			}
			int sanctionedMinutes = sanctionedMinutesFromFullDays + sanctionedMinutesFromPartialDays;

			return sanctionedMinutes;
		}

		private TimeSpan CalculateRestTime (int restMinutes)
		{
			int days = (int)Math.Floor ((double)restMinutes / 600);
			int minutes = restMinutes - (days * 600);

			return new TimeSpan (days, 0, minutes, 0);
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

