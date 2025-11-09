
using KioskApi2.Clothing;

namespace KioskApi2.Configuration;

public static class OptionsConfiguration
{
	public static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder)
	{
		var config = builder.Configuration;

		builder.Services.AddOptions<PersonOptions>()
			.Bind(config.GetRequiredSection(PersonOptions.Location));

		builder.Services.AddOptions<Clothing.Clothing>()
			.Bind(config.GetRequiredSection(Clothing.Clothing.Location));

		builder.Services.AddOptions<Adjustments>()
			.Bind(config.GetRequiredSection(Adjustments.Location));

		return builder;
	}
}