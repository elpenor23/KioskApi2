
using KioskApi2.HttpClients;
using KioskApi2.Clothing;
using KioskApi2.DataAccess;
using KioskApi2.IndoorStatus;
using KioskApi2.Moon;
using KioskApi2.Solar;
using KioskApi2.Weather;

using Serilog;

var AllowAllCORS = "_MyAllowAllCORS";

var builder = WebApplication.CreateBuilder(args);

// Not sure if this is the best way to do this.
// But this ensures that we have a place for things that need to run on startup
// to initilze things like databases and stuff like that.
//await KioskApi2.Managers.StartUpManager.Startup(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowAllCORS,
        policy =>
        {
            policy.WithOrigins("*");
        });
});

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureHttpClients();
builder.Services.AddMemoryCache();

var config = builder.Configuration;
builder.Services.AddOptions<PersonOptions>()
    .Bind(config.GetRequiredSection(PersonOptions.Location));
builder.Services.AddOptions<Clothing>()
    .Bind(config.GetRequiredSection(Clothing.Location));
builder.Services.AddOptions<Adjustments>()
    .Bind(config.GetRequiredSection(Adjustments.Location));

builder.Services.AddTransient<IMoonPhaseManager, MoonPhaseManager>();
builder.Services.AddTransient<IClothingManager, ClothingManager>();
builder.Services.AddTransient<IIndoorStatusManager, IndoorStatusManager>();
builder.Services.AddTransient<ISolarManager, SolarManager>();
builder.Services.AddTransient<IWeatherManager, WeatherManager>();
builder.Services.AddTransient<IDatabaseManager, DatabaseManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseCors(AllowAllCORS);
app.UseAuthorization();

app.MapControllers();

app.Run();
