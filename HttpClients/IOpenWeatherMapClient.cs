
using KioskApi2.HttpClients.Models;

namespace KioskApi2.HttpClients;

public interface IOpenWeatherMapClient
{
	Task<OpenWeatherMap?> GetCurrentWeather(decimal lat, decimal lon);
	Task<string> GetCurrentWeatherString(decimal lat, decimal lon);
}