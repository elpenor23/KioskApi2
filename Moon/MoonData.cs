namespace KioskApi2.Moon;

public class MoonData
{
	private const string TIME_FORMAT = "h:mm tt";
	public MoonData() { }

	public int? PhaseIndex { get; set; }
	public string? PhaseName { get; set; }
	public string? PhaseIcon { get; set; }
	public string? DayLength { get; set; }
	public DateTime? SunriseTime { get; set; }
	public DateTime? SunsetTime { get; set; }
	public string? DayTimeFormatted
	{
		get
		{
			return SunriseTime?.ToString(TIME_FORMAT) + " - " + SunsetTime?.ToString(TIME_FORMAT);
		}
	}

	//Additional constructors
	public MoonData(int index, string name, string icon, string length, DateTime? sunrise, DateTime sunset)
	{
		PhaseIndex = index;
		PhaseName = name;
		PhaseIcon = icon;
		DayLength = length;
		SunriseTime = sunrise;
		SunsetTime = sunset;
	}
	public MoonData(double moonPhase, DateTime sunrise, DateTime sunset)
	{
		var index = GetPhaseIndex(moonPhase);
		var data = GetPhaseData(index);
		var dayLength = GetDayLength(sunrise, sunset);

		PhaseIndex = index;
		PhaseName = data.Item1;
		PhaseIcon = data.Item2;
		DayLength = dayLength;
		SunriseTime = sunrise;
		SunsetTime = sunset;

	}

	//static helper methods
	private static int GetPhaseIndex(double moonPhase)
	{
		int index = 0;
		if (moonPhase == 0 || moonPhase == 1)
		{
			index = 0;
		}
		else if (moonPhase > 0 && moonPhase < 0.25D)
		{
			index = 1;
		}
		else if (moonPhase == 0.25D)
		{
			index = 2;
		}
		else if (moonPhase > 0.25D && moonPhase < 0.5D)
		{
			index = 3;
		}
		else if (moonPhase == 0.5D)
		{
			index = 4;
		}
		else if (moonPhase > 0.5D && moonPhase < 0.75D)
		{
			index = 5;
		}
		else if (moonPhase == 0.75D)
		{
			index = 6;
		}
		else if (moonPhase > 0.75D && moonPhase < 1D)
		{
			index = 7;
		}

		return index;
	}

	private static Tuple<string, string> GetPhaseData(int moonIndex)
	{
		string icon = string.Empty;
		string name = string.Empty;

		switch (moonIndex)
		{
			case 0:
				icon = "new-moon";
				name = "New Moon";
				break;
			case 1:
				icon = "waxing-crescent-moon";
				name = "Waxing Crescent Moon";
				break;
			case 2:
				icon = "first-quarter-moon";
				name = "First Quarter Moon";
				break;
			case 3:
				icon = "waxing-gibbous-moon";
				name = "Waxing Gibbous Moon";
				break;
			case 4:
				icon = "full-moon";
				name = "Full Moon";
				break;
			case 5:
				icon = "waning-gibbous-moon";
				name = "Waning Gibbous Moon";
				break;
			case 6:
				icon = "last-quarter-moon";
				name = "Last Quarter Moon";
				break;
			case 7:
				icon = "waning-crescent-moon";
				name = "Waning Crescent Moon";
				break;
		}

		return new Tuple<string, string>(name, icon);
	}

	private static string GetDayLength(DateTime sunrise, DateTime sunset)
	{
		TimeSpan span = sunset - sunrise;
		return String.Format("{0} hours, {1} minutes", span.Hours, span.Minutes);
	}
}