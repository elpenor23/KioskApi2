// 
// using SQLite;

// namespace KioskApi2.Managers;

// public static class StartUpManager
// {
//     public static async Task Startup(IConfiguration configuration)
//     {
//         //THINGS IN HERE WILL RUN ON EVERY START UP
//         //PLEASE ENSURE THAT METHODS ARE IDEMPOTENT (AKA RE-RUNNABLE)

//         // Ensures we have a database and all of our tables.
//         await InitializeDatabase(configuration);
//     }
//     private static async Task InitializeDatabase(IConfiguration configuration)
//     {

//         var databaseId = configuration["DatabaseId"] ?? "CachedData";
//         var database = new SQLiteAsyncConnection(databaseId);

//         var tablesExist = await TablesExist(database);

//         if (!tablesExist)
//         {
//             await database.CreateTableAsync<WeatherItem>();
//             await database.CreateTableAsync<IndoorStatusData>();
//             await database.CreateTableAsync<MoonData>();
//             await database.CreateTableAsync<SolarData>();
//         }

//         return;
//     }

//     private static async Task<bool> TablesExist(SQLiteAsyncConnection database)
//     {
//         var results = await AsyncTableQuery<int>.RunSelectQuery(
//             database,
//             $"SELECT * FROM sqlite_master WHERE type='table' AND (name='WeatherItem' OR name='IndoorStatusData' OR name='MoonData' OR name='SolarData')"
//             );

//         return results.Count == 4;
//     }
// }