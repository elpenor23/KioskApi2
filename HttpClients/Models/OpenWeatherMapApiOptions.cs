namespace KioskApi2.HttpClients.Models;

public class OpenWeatherMapApiOptions : ApiOptions
{
	public static readonly string Location = "HttpClients:OpenWeatherMapApi";
	public required OpenWeatherMapApiEndPoints EndPoints { get; set; }
	public OpenWeatherMapApiQueryStringData? QueryStringData { get; set; }
}

public class OpenWeatherMapApiEndPoints
{
	public required string OneCall3 { get; set; }
}

public class OpenWeatherMapApiQueryStringData
{
	public string Lang { get; set; } = string.Empty;
	public string Units { get; set; } = string.Empty;
	public string Exclude { get; set; } = string.Empty;
}