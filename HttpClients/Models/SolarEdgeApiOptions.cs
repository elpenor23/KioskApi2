
namespace KioskApi2.HttpClients.Models;

public class SolarEdgeApiOptions : ApiOptions
{
	public static readonly string Location = "HttpClients:SolarEdgeApi";
	public required SolarEdgeApiEndPoints EndPoints { get; set; }
	public required SolarEdgeApiQueryStringData? QueryStringData { get; set; }
}

public class SolarEdgeApiEndPoints
{
	public required string SiteOverView { get; set; }
	public required string HealthCheck { get; set; }
}

public class SolarEdgeApiQueryStringData
{
	public required string SiteId { get; set; }
}