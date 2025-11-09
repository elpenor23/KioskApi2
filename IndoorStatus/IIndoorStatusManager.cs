

namespace KioskApi2.IndoorStatus
{
	public interface IIndoorStatusManager
	{
		Task<IndoorStatusData> GetIndoorStatus();
		Task<IndoorStatusData> SaveIndoorStatus(string status);
	}
}