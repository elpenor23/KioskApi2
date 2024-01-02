
using KioskApi2.Models;
using SQLite;

namespace KioskApi2.Managers;
public class DatabaseManager
{
    #region "Database Access"
    readonly string databaseId;
    public IConfiguration Configuration { get; }
    private readonly SQLiteAsyncConnection database;

    public DatabaseManager(IConfiguration configuration)
    {
        Configuration = configuration;
        databaseId = Configuration["DatabaseId"] ?? "CachedData";
        database = new SQLiteAsyncConnection(databaseId);
    }

    public async Task<IEnumerable<WeatherItem>> GetWeatherData(int maxResults)
    {
        var results = await AsyncTableQuery<WeatherItem>.RunSelectQuery(
            database,
            $"SELECT * FROM WeatherItem");

        return results;

    }

    public async Task<IEnumerable<IndoorStatusData>> GetIndoorStatusData()
    {
        var results = await AsyncTableQuery<IndoorStatusData>.RunSelectQuery(
            database,
            $"SELECT * FROM IndoorStatusData");

        return results;

    }

    public async Task<IEnumerable<SolarData>> GetSolarData()
    {
        var results = await AsyncTableQuery<SolarData>.RunSelectQuery(
            database,
            $"SELECT * FROM SolarData");

        return results;
    }

    public async void AddUpdateData<T>(T data)
    {
        try
        {
            await database.RunInTransactionAsync(tran =>
            {
                tran.InsertOrReplace(data);
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    #endregion
}

public static class AsyncTableQuery<T> where T : new()
{
    public static async Task<List<T>> RunSelectQuery(SQLiteAsyncConnection database, string query)
    {
        try
        {
            var results = await database.QueryAsync<T>(query);

            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}