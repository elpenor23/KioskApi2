
using System.Text;

using KioskApi2.HttpClients.Models;

using Microsoft.Extensions.Options;

namespace KioskApi2.HttpClients;

public class OpenWeatherMapClient(HttpClient httpClient, IOptions<OpenWeatherMapApiOptions> options, Serilog.ILogger logger) : HttpClientBase(httpClient), IOpenWeatherMapClient
{
	private readonly OpenWeatherMapApiOptions _options = options.Value;
	public async Task<OpenWeatherMap?> GetCurrentWeather(decimal lat, decimal lon)
	{
		var endpoint = GetOneCall3EndPoint(lat, lon);

		logger.Information("Endpoint: {endpoint}", endpoint);

		var x = await GetAsync<OpenWeatherMap>(endpoint);

		return x;
	}

	public async Task<string> GetCurrentWeatherString(decimal lat, decimal lon)
	{
		var endpoint = GetOneCall3EndPoint(lat, lon);

		logger.Information("Endpoint: {endpoint}", endpoint);

		var x = await GetAsyncString(endpoint);

		return x;
	}
	private string GetOneCall3EndPoint(decimal lat, decimal lon)
	{
		var queryStringData = _options?.QueryStringData ?? throw new Exception("Incomplete Open Weather Map API Configuration: QueryStringData");
		var appId = _options?.HeaderData?.AuthToken ?? throw new Exception("Incomplete Open Weather Map API Configuration: AuthToken");

		var sb = new StringBuilder(_options.EndPoints.OneCall3);
		sb.Append(_options.QueryString);

		var x = sb.ToString();
		var y = string.Format(x, appId, lat, lon, queryStringData.Lang, queryStringData.Units, queryStringData.Exclude);

		return y;
	}
}