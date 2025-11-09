using HealthChecks.UI.Client;

using KioskApi2.HealthChecks;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace KioskApi2.Configuration;

public static class HealthCheckConfiguration
{
	public static WebApplicationBuilder ConfigureHealthChecks(this WebApplicationBuilder builder)
	{
		builder.Services.AddHealthChecks()
			.AddCheck<OpenWeatherMapHealthCheck>("check-OpenWeatherMapApi", HealthStatus.Unhealthy)
			.AddCheck<SolarEdgeHealthCheck>("check-SolarEdgeApi", HealthStatus.Unhealthy);

		return builder;
	}

	public static WebApplication AddHeathChecks(this WebApplication app)
	{
		app.MapGet("/health", () => "Healthy");

		app.MapHealthChecks("/health-status",
			new HealthCheckOptions
			{
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			});

		return app;
	}
}