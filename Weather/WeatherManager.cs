using KioskApi2.HttpClients.Models;
using KioskApi2.HttpClients;
using Microsoft.Extensions.Caching.Memory;

namespace KioskApi2.Weather;
public class WeatherManager(Serilog.ILogger logger, IOpenWeatherMapClient weatherMapClient, IMemoryCache memoryCache) : IWeatherManager
{
    private readonly Serilog.ILogger _logger = logger;

    public async Task<WeatherItem> GetWeather(string latString, string lonString)
    {
        _logger.Information("Getting Weather! Lat: {lat} | Lon: {lon}", latString, lonString);
        if (!decimal.TryParse(latString, out decimal lat) || !decimal.TryParse(lonString, out decimal lon))
        {
            throw new Exception("Invalid Latitude or Longitude.");
        }

        //round lat/lon because weather api only deals with 4 decimals
        lat = Math.Round(lat, 4, MidpointRounding.ToZero);
        lon = Math.Round(lon, 4, MidpointRounding.ToZero);

        var key = lat.ToString() + lon.ToString();
        
        if (!memoryCache.TryGetValue(key, out OpenWeatherMap? openWeatherMap))
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            openWeatherMap = await weatherMapClient.GetCurrentWeather(lat, lon);
            if (openWeatherMap is not null)
                memoryCache.Set(key, openWeatherMap, cacheEntryOptions);
        }

        return (WeatherItem)openWeatherMap;
    }

    public async Task<string> GetWeatherTestString(string latString, string lonString)
    {
        _logger.Information("Getting Weather! Lat: {lat} | Lon: {lon}", latString, lonString);
        if (!decimal.TryParse(latString, out decimal lat) || !decimal.TryParse(lonString, out decimal lon))
        {
            throw new Exception("Invalid Latitude or Longitude.");
        }

        //round lat/lon because weather api only deals with 4 decimals
        lat = Math.Round(lat, 4, MidpointRounding.ToZero);
        lon = Math.Round(lon, 4, MidpointRounding.ToZero);

        return await weatherMapClient.GetCurrentWeatherString(lat, lon);
    }
}