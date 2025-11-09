using System.Text.Json.Serialization;

namespace KioskApi2.HttpClients.Models;


public class Current
{
	[JsonPropertyName("dt")]
	public long Dt { get; set; }

	[JsonPropertyName("sunrise")]
	public long Sunrise { get; set; }

	[JsonPropertyName("sunset")]
	public long Sunset { get; set; }

	[JsonPropertyName("temp")]
	public double Temp { get; set; }

	[JsonPropertyName("feels_like")]
	public double FeelsLike { get; set; }

	[JsonPropertyName("pressure")]
	public int Pressure { get; set; }

	[JsonPropertyName("humidity")]
	public int Humidity { get; set; }

	[JsonPropertyName("dew_point")]
	public double DewPoint { get; set; }

	[JsonPropertyName("uvi")]
	public double Uvi { get; set; }

	[JsonPropertyName("clouds")]
	public int Clouds { get; set; }

	[JsonPropertyName("visibility")]
	public int Visibility { get; set; }

	[JsonPropertyName("wind_speed")]
	public double WindSpeed { get; set; }

	[JsonPropertyName("wind_deg")]
	public int WindDeg { get; set; }

	[JsonPropertyName("wind_gust")]
	public double WindGust { get; set; }

	[JsonPropertyName("weather")]
	public required List<Weather> Weather { get; set; }
}

public class Daily
{
	[JsonPropertyName("dt")]
	public long Dt { get; set; }

	[JsonPropertyName("sunrise")]
	public long Sunrise { get; set; }

	[JsonPropertyName("sunset")]
	public long Sunset { get; set; }

	[JsonPropertyName("moonrise")]
	public long Moonrise { get; set; }

	[JsonPropertyName("moonset")]
	public long Moonset { get; set; }

	[JsonPropertyName("moon_phase")]
	public double MoonPhase { get; set; }

	[JsonPropertyName("summary")]
	public required string Summary { get; set; }

	[JsonPropertyName("temp")]
	public required Temp Temp { get; set; }

	[JsonPropertyName("feels_like")]
	public required FeelsLike FeelsLike { get; set; }

	[JsonPropertyName("pressure")]
	public int Pressure { get; set; }

	[JsonPropertyName("humidity")]
	public int Humidity { get; set; }

	[JsonPropertyName("dew_point")]
	public double DewPoint { get; set; }

	[JsonPropertyName("wind_speed")]
	public double WindSpeed { get; set; }

	[JsonPropertyName("wind_deg")]
	public int WindDeg { get; set; }

	[JsonPropertyName("wind_gust")]
	public double WindGust { get; set; }

	[JsonPropertyName("weather")]
	public List<Weather> Weather { get; set; } = [];

	[JsonPropertyName("clouds")]
	public int Clouds { get; set; }

	[JsonPropertyName("pop")]
	public double Pop { get; set; }

	[JsonPropertyName("uvi")]
	public double Uvi { get; set; }

	[JsonPropertyName("rain")]
	public double? Rain { get; set; }
}

public class FeelsLike
{
	[JsonPropertyName("day")]
	public double Day { get; set; }

	[JsonPropertyName("night")]
	public double Night { get; set; }

	[JsonPropertyName("eve")]
	public double Eve { get; set; }

	[JsonPropertyName("morn")]
	public double Morn { get; set; }
}

public class OpenWeatherMap
{
	[JsonPropertyName("lat")]
	public double Lat { get; set; }

	[JsonPropertyName("lon")]
	public double Lon { get; set; }

	[JsonPropertyName("timezone")]
	public required string Timezone { get; set; }

	[JsonPropertyName("timezone_offset")]
	public int TimezoneOffset { get; set; }

	[JsonPropertyName("current")]
	public required Current Current { get; set; }

	[JsonPropertyName("daily")]
	public required List<Daily> Daily { get; set; }
}

public class Temp
{
	[JsonPropertyName("day")]
	public double Day { get; set; }

	[JsonPropertyName("min")]
	public double Min { get; set; }

	[JsonPropertyName("max")]
	public double Max { get; set; }

	[JsonPropertyName("night")]
	public double Night { get; set; }

	[JsonPropertyName("eve")]
	public double Eve { get; set; }

	[JsonPropertyName("morn")]
	public double Morn { get; set; }
}

public class Weather
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("main")]
	public required string Main { get; set; }

	[JsonPropertyName("description")]
	public required string Description { get; set; }

	[JsonPropertyName("icon")]
	public required string Icon { get; set; }
}