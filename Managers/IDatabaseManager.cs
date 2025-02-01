using KioskApi2.Models;

namespace KioskApi2.Managers
{
    public interface IDatabaseManager
    {
        Task<IEnumerable<WeatherItem>> GetWeatherData(int maxResults);
        Task<IEnumerable<IndoorStatusData>> GetIndoorStatusData();
        Task<IEnumerable<SolarData>> GetSolarData();
        void AddUpdateData<T>(T data);
    }
}