
using KioskApi.Models;
using SQLite;

namespace KioskApi.Managers;
public class DatabaseManager
{    
    #region "Database Access"
    readonly string databaseId;
    public IConfiguration Configuration{get;}
    private readonly SQLiteAsyncConnection database;

    public DatabaseManager(IConfiguration configuration){
        Configuration = configuration;
        databaseId = Configuration["DatabaseId"] ?? "CachedData";
        database = new SQLiteAsyncConnection(databaseId);
    }

    public async Task<IEnumerable<WeatherItem>> GetWeatherData(int maxResults)
    {
        try
        {
            var results = await database.QueryAsync<WeatherItem>($"SELECT * FROM WeatherItem");

            return results;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<IndoorStatusData>> GetIndoorStatusData()
    {
        try
        {
            var results = await database.QueryAsync<IndoorStatusData>($"SELECT * FROM IndoorStatusData");

            return results;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
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
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    #endregion

    public async Task InitializeDatabase()
    {
        var tablesExist = await TablesExist();
        
        if (!tablesExist)
        {
            Console.WriteLine("*****Initializing TABLES!!");
            await database.CreateTableAsync<WeatherItem>();
            await database.CreateTableAsync<IndoorStatusData>();
            await database.CreateTableAsync<MoonData>();
        }

        return;
    }

    private async Task<bool> TablesExist()
    {
        try
        {
            var results = await database.QueryAsync<int>($"SELECT * FROM sqlite_master WHERE type='table' AND (name='WeatherItem' OR name='IndoorStatusData' OR name='MoonData')");
            return results.Count == 3;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}