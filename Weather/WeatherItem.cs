using KioskApi2.Enums;
using KioskApi2.HttpClients.Models;

namespace KioskApi2.Weather;

public class WeatherItem()
{
	const string Degree_sign = "°";

	public double? Lat { get; set; }

	public double? Lon { get; set; }

	public DateTime? WeatherTime { get; set; }
	public string? WeatherTimeFormatted
	{
		get
		{
			return WeatherTime?.ToString("MM/dd/yyyy HH:mm:ss");
		}
	}
	public double? CurrentTemp { get; set; }
	public string? CurrentTempFormatted
	{
		get
		{
			return string.Format("{0}{1}", CurrentTemp.ToString(), Degree_sign);
		}
	}
	public double? CurrentDewPoint { get; set; }
	public double? CurrentFeelsLike { get; set; }
	public string? CurrentFeelsLikeFormatted
	{
		get
		{
			return string.Format("{0}{1}", CurrentFeelsLike.ToString(), Degree_sign);
		}
	}
	public double? CurrentWindSpeed { get; set; }
	public string? CurrentIconId { get; set; }
	public string? CurrentMain { get; set; } //TODO: GIVE THIS A BETTER NAME
	public double? TodayMinTemp { get; set; }
	public string? TodayMinTempFormatted
	{
		get
		{
			return string.Format("{0}{1}", TodayMinTemp.ToString(), Degree_sign);
		}
	}
	public double? TodayMaxTemp { get; set; }
	public string? TodayMaxTempFormatted
	{
		get
		{
			return string.Format("{0}{1}", TodayMaxTemp.ToString(), Degree_sign);
		}
	}
	public string? TodayDescription { get; set; }
	public string? TodaySummary
	{
		get
		{
			return string.Format("Today: {0}\n Low: {1} / High: {2}", TodayDescription, TodayMinTemp, TodayMaxTemp);
		}
	}
	public double? TomorrowMinTemp { get; set; }
	public string? TomorrowMinTempFormatted
	{
		get
		{
			return string.Format("{0}{1}", TomorrowMinTemp.ToString(), Degree_sign);
		}
	}
	public double? TomorrowMaxTemp { get; set; }
	public string? TomorrowMaxTempFormatted
	{
		get
		{
			return string.Format("{0}{1}", TomorrowMaxTemp.ToString(), Degree_sign);
		}
	}
	public string? TomorrowDescription { get; set; }
	public string? TomorrowForecast
	{
		get
		{
			return string.Format("Tomorrow: {0}\nLow: {1} / High: {2}", TomorrowDescription, TomorrowMinTempFormatted, TomorrowMaxTempFormatted);
		}
	}
	public DateTime? SunriseTime { get; set; }
	public DateTime? SunsetTime { get; set; }
	public double? MoonPhase { get; set; }
	public TimeOfDay TimeOfDay
	{
		get
		{
			return this.GetTimeOfDay();
		}
	}

	private TimeOfDay GetTimeOfDay()
	{
		if (this.WeatherTime == null || this.SunriseTime == null || this.SunsetTime == null)
		{
			return TimeOfDay.Unknown;
		}

		if (IsDawn(this.WeatherTime.Value, this.SunriseTime.Value))
		{
			return TimeOfDay.Day;
		}
		else if (IsDusk(this.WeatherTime.Value, this.SunsetTime.Value))
		{
			return TimeOfDay.Dusk;
		}
		else if (IsDay(this.WeatherTime.Value, this.SunriseTime.Value, this.SunsetTime.Value))
		{
			return TimeOfDay.Day;
		}
		else if (IsNight(this.WeatherTime.Value, this.SunsetTime.Value))
		{
			return TimeOfDay.Night;
		}
		else
		{
			return TimeOfDay.Unknown;
		}
	}

	public static implicit operator WeatherItem(OpenWeatherMap? openWeatherMap)
	{
		if (openWeatherMap is null)
			return new WeatherItem();

		//44.176097, -70.679317

		return new WeatherItem
		{
			Lat = openWeatherMap.Lat,
			Lon = openWeatherMap.Lon,

			WeatherTime = ConvertEpochTimeToDateTime(openWeatherMap.Current.Dt),
			SunriseTime = ConvertEpochTimeToDateTime(openWeatherMap.Current.Sunrise),
			SunsetTime = ConvertEpochTimeToDateTime(openWeatherMap.Current.Sunset),

			CurrentTemp = openWeatherMap.Current.Temp,
			CurrentDewPoint = openWeatherMap.Current.DewPoint,
			CurrentWindSpeed = openWeatherMap.Current.WindSpeed,
			CurrentFeelsLike = openWeatherMap.Current.FeelsLike,
			CurrentIconId = openWeatherMap.Current.Weather[0].Icon,
			CurrentMain = openWeatherMap.Current.Weather[0].Main,

			TodayMaxTemp = openWeatherMap.Daily[0].Temp.Max,
			TodayMinTemp = openWeatherMap.Daily[0].Temp.Min,
			TodayDescription = openWeatherMap.Daily[0].Summary,

			TomorrowMaxTemp = openWeatherMap.Daily[1].Temp.Max,
			TomorrowMinTemp = openWeatherMap.Daily[1].Temp.Min,
			TomorrowDescription = openWeatherMap.Daily[1].Summary,

			MoonPhase = openWeatherMap.Daily[0].MoonPhase,
		};
	}

	private static decimal ConvertDoubleToDecimal(double dbl, int numDecimalPoints = 2)
	{
		return decimal.Round(Convert.ToDecimal(dbl), numDecimalPoints);
	}

	private static DateTime ConvertEpochTimeToDateTime(Int64 timestamp)
	{
		var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);

		var dateTime = dateTimeOffset.DateTime;

		var localTime = dateTime.ToLocalTime();

		return localTime;
	}

	private static bool IsDay(DateTime currentTime, DateTime sunRiseTime, DateTime sunSetTime)
	{
		return sunRiseTime <= currentTime && currentTime <= sunSetTime;
	}

	private static bool IsDawn(DateTime currentTime, DateTime sunRiseTime)
	{
		var thirtyMinutesBeforeSunrise = sunRiseTime.AddMinutes(-30);
		var thirtyMinutesAfterSunrise = sunRiseTime.AddMinutes(30);

		return thirtyMinutesBeforeSunrise <= currentTime && currentTime <= thirtyMinutesAfterSunrise;
	}

	private static bool IsDusk(DateTime currentTime, DateTime sunSetTime)
	{
		var thirtyMinutesBeforeSunset = sunSetTime.AddMinutes(-30);
		var thirtyMinutesAfterSunset = sunSetTime.AddMinutes(30);

		return thirtyMinutesBeforeSunset <= currentTime && currentTime <= thirtyMinutesAfterSunset;
	}

	private static bool IsNight(DateTime currentTime, DateTime sunSetTime)
	{
		return currentTime >= sunSetTime;
	}

	public static implicit operator HttpContent(WeatherItem v)
	{
		throw new NotImplementedException();
	}
}