
using System.Text;

using KioskApi2.HttpClients.Models;
using KioskApi2.Solar;

using Microsoft.Extensions.Options;

namespace KioskApi2.HttpClients;

public class SolarEdgeApiClient(HttpClient httpClient, IOptions<SolarEdgeApiOptions> options, Serilog.ILogger logger) : HttpClientBase(httpClient), ISolarEdgeApiClient
{
	private readonly SolarEdgeApiOptions _options = options.Value;
	public async Task<SolarData?> GetSolarData()
	{
		var endPoint = GetSiteOverViewEndPoint();

		logger.Information("Solar endpoint: {endpoint}", endPoint);

		var x = await GetAsync<SolarData>(endPoint);

		return x;
	}

	public async Task HealthCheck()
	{
		var endPoint = GetSiteOverViewEndPoint();

		logger.Information("Solar endpoint: {endpoint}", endPoint);

		await GetAsync<SolarData>(endPoint);
	}

	private string GetSiteOverViewEndPoint()
	{
		var queryStringData = _options?.QueryStringData ?? throw new Exception("Incomplete Open Weather Map API Configuration: QueryStringData");
		var appId = _options?.HeaderData?.AuthToken ?? throw new Exception("Incomplete Open Weather Map API Configuration: AuthToken");

		var sb = new StringBuilder(_options.EndPoints.SiteOverView);
		sb.Append(_options.QueryString);

		var x = sb.ToString();
		var y = string.Format(x, queryStringData.SiteId, appId);

		return y;
	}
}