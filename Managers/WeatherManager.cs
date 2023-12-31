using System.Net.Http.Headers;
using KioskApi.Models;

namespace KioskApi.Managers;
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
        await Dbm.InitializeDatabase();
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
        var result = "[{}]";
        var uri = String.Format(Configuration["WeatherApi:weather_req_url"] ?? string.Empty,
                Configuration["WeatherApi:weather_api_token"],
                lat,
                lon,
                Configuration["WeatherApi:weather_lang"],
                Configuration["WeatherApi:weather_unit"],
                Configuration["WeatherApi:weather_exclude_list"]);

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorMessage = string.Format($"Error Getting data from {uri}. Status Code: {response.StatusCode}");
                throw new Exception(errorMessage);
            }
        }

        return result;
    }
    #endregion
}