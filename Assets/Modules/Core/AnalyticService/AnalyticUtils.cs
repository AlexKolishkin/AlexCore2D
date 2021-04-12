using System;

namespace Core.analytic
{
	public static class AnalyticUtils
	{
		public static string ToTextMark(this long seconds)
		{
			if (seconds < 0)
			{
				seconds = 0;
			}

			TimeSpan ts = TimeSpan.FromSeconds(seconds);
			return ts.ToTextMark();
		}

		public static string ToTextMark(this TimeSpan ts)
		{
			double seconds = ts.TotalSeconds;
			if (seconds >= 0.0 && seconds < 60)
			{
				return "0-60 sec";
			}

			if (60 <= seconds && seconds < 5 * 60)
			{
				return "1-5 min";
			}

			if (5 * 60 <= seconds && seconds < 30 * 60)
			{
				return "5-30 min";
			}

			if (30 * 60 <= seconds && seconds < 60 * 60)
			{
				return "30-60 min";
			}

			if (3600 <= seconds && seconds < 7200)
			{
				return "1-2 h";
			}

			if (7200 <= seconds && seconds < 18000)
			{
				return "2-5 h";
			}

			if (18000 <= seconds && seconds < 36000)
			{
				return "5-10 h";
			}

			if (36000 <= seconds && seconds < 57600)
			{
				return "10-16 h";
			}

			if (57600 <= seconds && seconds < 86400)
			{
				return "16-24 h";
			}

			if (86400 <= seconds && seconds < 129600)
			{
				return "24-36 h";
			}

			if (129600 <= seconds && seconds < 172800)
			{
				return "36-48 h";
			}

			if (172800 <= seconds && seconds < 259200)
			{
				return "48-72 h";
			}

			if (259200 <= seconds && seconds < 345600)
			{
				return "72-96 h";
			}

			if (345600 <= seconds && seconds < 518400)
			{
				return "4-6 d";
			}

			if (518400 <= seconds && seconds < 950400)
			{
				return "7-11 d";
			}

			return "> 1 w";
		}

		public static string ToOrdinalNumber(this int number)
		{
			switch (number)
			{
				case 1:
					return "First";
				case 2:
					return "Second";
				case 3:
					return "Third";
				case 4:
					return "Fourth";
				case 5:
					return "Fifth";
				case 6:
					return "Sixth";
				case 7:
					return "Seventh";
				case 8:
					return "Eighth";
				case 9:
					return "Ninth";
				case 10:
					return "Tenth";
			}

			return number.ToString();
		}
	}
}