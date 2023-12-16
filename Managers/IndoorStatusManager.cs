using KioskApi.Models;
namespace KioskApi.Managers;
public class IndoorStatusManager(IConfiguration configuration)
{
    private readonly DatabaseManager dbm = new(configuration);

    public async Task<IndoorStatusData> GetIndoorStatus()
    {
        var statusData = await dbm.GetIndoorStatusData();
        var data = statusData.Where(x => x.Id == "1").ToList().FirstOrDefault() ?? new IndoorStatusData();
        return data;
    }

    public async Task<IndoorStatusData> SaveIndoorStatus(string status)
    {
        var threeMinutesAgo = DateTime.Now.AddMinutes(-3);
        var ReturnData = new IndoorStatusData();
        var statusData = await dbm.GetIndoorStatusData();

        //get cached data
        var data = statusData.Where(x => x.Id == "1").ToList().FirstOrDefault() ?? new IndoorStatusData();
        data.Data = status;
        data.LastSet = DateTime.Now;

        dbm.AddUpdateData(data);

        return data;
    }
}