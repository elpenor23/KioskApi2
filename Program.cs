using KioskApi2.Managers;
using Serilog;

var AllowAllCORS = "_MyAllowAllCORS";

var builder = WebApplication.CreateBuilder(args);

// Not sure if this is the best way to do this.
// But this ensures that we have a place for things that need to run on startup
// to initilze things like databases and stuff like that.
await KioskApi2.Managers.StartUpManager.Startup(builder.Configuration);

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

builder.Services.AddSingleton<IMoonPhaseManager, MoonPhaseManager>();
builder.Services.AddSingleton<IClothingManager, ClothingManager>();
builder.Services.AddSingleton<IIndoorStatusManager, IndoorStatusManager>();
builder.Services.AddSingleton<ISolarManager, SolarManager>();
builder.Services.AddSingleton<IWeatherManager, WeatherManager>();
builder.Services.AddSingleton<IDatabaseManager, DatabaseManager>();

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
