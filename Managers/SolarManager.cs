using KioskApi2.Models;
using KioskApi2.Utilities;
using Newtonsoft.Json;

namespace KioskApi2.Managers;

public class SolarManager(IConfiguration configuration)
{
    private DatabaseManager dbm = new(configuration);
    public DatabaseManager Dbm { get => dbm; set => dbm = value; }
    private IConfiguration Configuration { get; } = configuration ?? throw new Exception("Configuration Error");

    public async Task<SolarData> GetSolarData()
    {
        var data = await GetSolarDataFromCache();

        //This needs to be a negative number since we are checking for X minutes AGO
        if (!double.TryParse(Configuration["SolarApi:solar_data_cache_time_minutes"], out double cache_time))
        {
            cache_time = -5;
        }

        var xMinutesAgo = DateTime.Now.AddMinutes(cache_time);

        if (data == null || data.CacheLastUpdated < xMinutesAgo)
        {
            data = await GetSolarDataFromApi();
            SaveSolarData(data);
        }
        return data;
    }

    private async Task<string> GetRawSolarDataFromApi()
    {
        var uri = String.Format(Configuration["SolarApi:base_req_url"] ?? string.Empty,
                Configuration["SolarApi:solar_site_id"],
                Configuration["SolarApi:solar_api_token"]);

        var result = await ApiUtils.GetApiJsonData(uri);

        return result;
    }
    private async Task<SolarData> GetSolarDataFromApi()
    {
        var rawData = await GetRawSolarDataFromApi();

        var solarData = ParseJsonSolarData(rawData);

        var solarMaxProduction = Configuration["SolarMaxProduction"] ?? "1";

        solarData.MaxPower = double.Parse(solarMaxProduction);

        return solarData;

    }
    private async Task<SolarData> GetSolarDataFromCache()
    {
        var solarData = await Dbm.GetSolarData();
        var data = solarData.Where(x => x.Id == 1).ToList().FirstOrDefault() ?? new SolarData();
        return data;
    }

    private async void SaveSolarData(SolarData sd)
    {
        var cached = await GetSolarDataFromCache();

        cached = sd;

        Dbm.AddUpdateData(cached);
    }

    private static SolarData ParseJsonSolarData(string jsonString)
    {
        dynamic? jsonSolarData = JsonConvert.DeserializeObject(jsonString);

        if (jsonSolarData == null) { return new SolarData(); }

        DateTime.TryParse(jsonSolarData["overview"]["lastUpdateTime"].Value, out DateTime lastUpdateTime);

        var solarData = new SolarData
        {
            Id = 1,
            MeasuredBy = jsonSolarData["overview"]["measuredBy"] ?? string.Empty,
            LastUpdateTime = lastUpdateTime,
            LifeTimeEnergy = jsonSolarData["overview"]["lifeTimeData"]["energy"],
            LastYearEnergy = jsonSolarData["overview"]["lastYearData"]["energy"],
            LastMonthEnergy = jsonSolarData["overview"]["lastMonthData"]["energy"],
            LastDayEnergy = jsonSolarData["overview"]["lastDayData"]["energy"],
            CurrentPower = jsonSolarData["overview"]["currentPower"]["power"],
            CacheLastUpdated = DateTime.Now
        };

        return solarData;
    }
}
