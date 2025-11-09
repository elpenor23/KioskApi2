
using KioskApi2.DataAccess;
namespace KioskApi2.IndoorStatus;
public class IndoorStatusManager(IConfiguration configuration, Serilog.ILogger logger) : IIndoorStatusManager
{
	private readonly DatabaseManager _dbm = new(configuration);
	private readonly Serilog.ILogger _logger = logger;

	public async Task<IndoorStatusData> GetIndoorStatus()
	{
		_logger.Debug("Getting IndoorStatusData");

		var statusData = await _dbm.GetIndoorStatusData();
		var data = statusData.Where(x => x.Id == "1").ToList().FirstOrDefault() ?? new IndoorStatusData();
		return data;
	}

	public async Task<IndoorStatusData> SaveIndoorStatus(string status)
	{
		var threeMinutesAgo = DateTime.Now.AddMinutes(-3);
		var returnData = new IndoorStatusData();
		var statusData = await _dbm.GetIndoorStatusData();

		//get cached data
		var data = statusData.Where(x => x.Id == "1").ToList().FirstOrDefault() ?? new IndoorStatusData();
		data.Data = status;
		data.LastSet = DateTime.Now;

		_dbm.AddUpdateData(data);

		return data;
	}
}