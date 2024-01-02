using Microsoft.AspNetCore.Mvc;
using KioskApi2.Models;
using KioskApi2.Managers;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using System.Net;
using System.Net.Http;

namespace KioskApi2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeatherController(IConfiguration configuration) : ControllerBase
{
    private readonly WeatherManager weatherManager = new(configuration);

    [HttpGet]
    public async Task<ActionResult<WeatherItem>> Get([FromQuery] string lat, [FromQuery] string lon)
    {
        var data = new WeatherItem();

        try
        {
            data = await weatherManager.GetWeather(lat, lon);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Weather Controller", ex.Message);
            return BadRequest(ModelState);
        }

        return data;

    }
}