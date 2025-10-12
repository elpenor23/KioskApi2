
using KioskApi2.Weather;

namespace KioskApi2.Moon;
public class MoonPhaseManager(IWeatherManager weatherManager) : IMoonPhaseManager
{
    public async Task<MoonData> GetMoonPhase(string lat, string lon)
    {
        var weather = await weatherManager.GetWeather(lat, lon);

        if (weather.MoonPhase == null || weather.SunriseTime == null || weather.SunsetTime == null) 
            return new MoonData(4, "Full Moon", "full-moon", "24 Hours", DateTime.Now, DateTime.Now); 

        var moonItem = new MoonData(weather.MoonPhase.Value, weather.SunriseTime.Value, weather.SunsetTime.Value);

        return moonItem;
    }
}