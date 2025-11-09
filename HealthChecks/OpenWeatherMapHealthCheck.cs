using KioskApi2.HttpClients;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace KioskApi2.HealthChecks;

public class OpenWeatherMapHealthCheck(IOpenWeatherMapClient client) : IHealthCheck
{
	public async Task<HealthCheckResult> CheckHealthAsync(
		HealthCheckContext context,
		CancellationToken cancellationToken = default)
	{
		try
		{
			await client.HealthCheck();
			return HealthCheckResult.Healthy();
		}
		catch (Exception ex)
		{
			return HealthCheckResult.Unhealthy(
				ex.Message,
				exception: ex);
		}
	}
}