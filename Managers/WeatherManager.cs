using KioskApi2.Utilities;
using KioskApi2.Models;

namespace KioskApi2.Managers;
public class WeatherManager(IConfiguration configuration)
{
    private DatabaseManager dbm = new(configuration);

    public DatabaseManager Dbm { get => dbm; set => dbm = value; }
    private IConfiguration Configuration { get; } = configuration;

    public async Task<WeatherItem> GetWeather(string latString, string lonString)
    {
        if (!decimal.TryParse(latString, out decimal lat) || !decimal.TryParse(lonString, out decimal lon))
        {
            throw new Exception("Invalid Latitude or Longitude.");
        }

        //round lat/lon because weather api only deals with 4 decimals
        lat = Math.Round(lat, 4, MidpointRounding.ToZero);
        lon = Math.Round(lon, 4, MidpointRounding.ToZero);

        //This needs to be a negative number since we are checking for X minutes AGO
        if (!double.TryParse(Configuration["WeatherApi:weather_data_cache_time_minutes"], out double cache_time))
        {
            cache_time = -3;
        }

        var xMinutesAgo = DateTime.Now.AddMinutes(cache_time);
        
        WeatherItem ReturnData = new WeatherItem();
        var weatherData = await Dbm.GetWeatherData(100);

        //get cached data
        var data = weatherData.Where(x => x.Lat == lat && x.Lon == lon).ToList().FirstOrDefault();

        // if the data does not exist or is old we need to refresh the data
        if (data == null || data.LastRefreshed < xMinutesAgo)
        {
            data ??= new WeatherItem();

            var rawWeatherData = await GetRawWeatherAsync(lat, lon);
            data.ProcessRawWeatherData(rawWeatherData);

            Dbm.AddUpdateData(data);

        }
        else
        {
            data.Type = "cached";
        }

        return data;
    }
    #region "Open Weather API Access"
    private async Task<string> GetRawWeatherAsync(decimal lat, decimal lon)
    {
        var uri = String.Format(Configuration["WeatherApi:weather_req_url"] ?? string.Empty,
                Configuration["WeatherApi:weather_api_token"],
                lat,
                lon,
                Configuration["WeatherApi:weather_lang"],
                Configuration["WeatherApi:weather_unit"],
                Configuration["WeatherApi:weather_exclude_list"]);

        var result = await ApiUtils.GetApiJsonData(uri);

        return result;
    }
    #endregion
}