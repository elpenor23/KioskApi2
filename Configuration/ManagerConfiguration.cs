using KioskApi2.Clothing;
using KioskApi2.DataAccess;
using KioskApi2.IndoorStatus;
using KioskApi2.Moon;
using KioskApi2.Solar;
using KioskApi2.Weather;

namespace KioskApi2.Configuration;

public static class ManagerConfiguration
{
	public static WebApplicationBuilder ConfigureManagers(this WebApplicationBuilder builder)
	{
		builder.Services.AddMemoryCache();

		builder.Services.AddTransient<IMoonPhaseManager, MoonPhaseManager>();
		builder.Services.AddTransient<IClothingManager, ClothingManager>();
		builder.Services.AddTransient<IIndoorStatusManager, IndoorStatusManager>();
		builder.Services.AddTransient<ISolarManager, SolarManager>();
		builder.Services.AddTransient<IWeatherManager, WeatherManager>();
		builder.Services.AddTransient<IDatabaseManager, DatabaseManager>();

		return builder;
	}
}