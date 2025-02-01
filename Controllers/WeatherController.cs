using Microsoft.AspNetCore.Mvc;
using KioskApi2.Models;
using KioskApi2.Managers;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Serilog;

namespace KioskApi2.Controllers;




[Route("api/[controller]")]
[ApiController]
public class WeatherController(IConfiguration configuration, Serilog.ILogger logger, IWeatherManager weatherManager) : ControllerBase
{
    private readonly Serilog.ILogger _logger = logger;
    private readonly IWeatherManager _weatherManager = weatherManager;
    

    [HttpGet]
    public async Task<ActionResult<WeatherItem>> Get([FromQuery] string lat, [FromQuery] string lon)
    {
        _logger.Debug("WeatherController - Getting Weather.");
        WeatherItem? data;

        try
        {
            data = await _weatherManager.GetWeather(lat, lon);

            _logger.Debug(data?.SunriseTime?.ToString() ?? "data sunrise time is null.");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Weather Controller", ex.Message);
            return BadRequest(ModelState);
        }

        return data;

    }
}