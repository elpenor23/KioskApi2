using Serilog;
namespace KioskApi2.Configuration;

public static class GenericConfiguration
{
	public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
	{
		builder.Host.UseSerilog((context, configuration) =>
			configuration.ReadFrom.Configuration(context.Configuration));

		builder.Services.AddControllers();

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddOpenApi();
		
		return builder;
	}
}