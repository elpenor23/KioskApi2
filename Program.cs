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
            policy.AllowAnyOrigin().AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(AllowAllCORS);
app.UseAuthorization();

app.MapControllers();

app.Run();
