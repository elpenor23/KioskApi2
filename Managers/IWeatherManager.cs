using KioskApi2.Models;

namespace KioskApi2.Managers
{
    public interface IWeatherManager
    {
        Task<WeatherItem> GetWeather(string latString, string lonString);
    }
}