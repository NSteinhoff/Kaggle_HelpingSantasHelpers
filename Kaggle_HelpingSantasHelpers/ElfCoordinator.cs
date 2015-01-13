using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaggle_HelpingSantasHelpers
{
	public static class ElfCoordinator
	{
		private static List<Elf> _elves = new List<Elf> ();

		public static List<Elf> Elves {
			get {
				return _elves.OrderBy (x => x.nextAvailable).ToList ();
			}
		}

		public static int HireElves (int numberOfElves)
		{
			for (int i = 0; i < numberOfElves; i++) {
				_elves.Add (new Elf (_elves.Count + 1));
			}

			return _elves.Count;
		}

		public static Elf PickNextElf ()
		{
			return Elves [0];
		}

		public static Elf PickFirstElf ()
		{
			return _elves [0];
		}

		public static Elf PickLastElf ()
		{
			return _elves [_elves.Count - 1];
		}
	}
}

