using KioskApi2.HttpClients.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace KioskApi2.HttpClients;

public static class HttpClientConfiguration
{
    public static WebApplicationBuilder ConfigureHttpClients(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<OpenWeatherMapApiOptions>(builder.Configuration.GetRequiredSection(OpenWeatherMapApiOptions.Location));
        builder.Services.Configure<SolarEdgeApiOptions>(builder.Configuration.GetRequiredSection(SolarEdgeApiOptions.Location));

        builder.Services.ConfigureHttpClientDefaults(builder =>
        {
            builder.AddStandardResilienceHandler();
            builder.ConfigureHttpClient(client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "KioskApi2");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
        });

        builder.Services.AddHttpClient<IOpenWeatherMapClient, OpenWeatherMapClient>((serviceProvider, client) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<OpenWeatherMapApiOptions>>().Value;

            client.BaseAddress = new Uri(settings.URL);
        });

        builder.Services.AddHttpClient<ISolarEdgeApiClient, SolarEdgeApiClient>((serviceProvider, client) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<SolarEdgeApiOptions>>().Value;

            client.BaseAddress = new Uri(settings.URL);
        });

        return builder;
    }
}
