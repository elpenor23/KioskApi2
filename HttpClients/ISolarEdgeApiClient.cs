using KioskApi2.Solar;

namespace KioskApi2.HttpClients;

public interface ISolarEdgeApiClient
{
	Task<SolarData?> GetSolarData();
}