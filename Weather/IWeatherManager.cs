

namespace KioskApi2.Weather
{
    public interface IWeatherManager
    {
        Task<WeatherItem> GetWeather(string latString, string lonString);
        Task<string> GetWeatherTestString(string latString, string lonString);
    }
}