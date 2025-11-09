using KioskApi2.Configuration;

using Scalar.AspNetCore;

using Serilog;

var allowAllCORS = "_MyAllowAllCORS";

var builder = WebApplication.CreateBuilder(args)
	.ConfigureServices()
	.ConfigureOptions()
	.ConfigureManagers()
	.ConfigureHttpClients()
	.ConfigureHealthChecks();

builder.Services.AddCors(options => options.AddPolicy(name: allowAllCORS,
		policy => policy.WithOrigins("*")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference(options => options
			.WithTitle("Kiosk API")
			.WithTheme(ScalarTheme.Mars)
			.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.AsyncHttp));
}

app.UseSerilogRequestLogging();

app.AddHeathChecks();

app.UseHttpsRedirection();
app.UseCors(allowAllCORS);
app.UseAuthorization();
app.MapControllers();

app.Run();