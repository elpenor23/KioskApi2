using System.Net;

using Microsoft.AspNetCore.Mvc;

using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace KioskApi2.Weather;

[Route("api/[controller]")]
[ApiController]
public class WeatherController(Serilog.ILogger logger, IWeatherManager weatherManager) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType((int)HttpStatusCode.NoContent)]
	[ProducesResponseType(typeof(WeatherItem), (int)HttpStatusCode.OK)]
	public async Task<IActionResult> Get([FromQuery] string lat, [FromQuery] string lon)
	{
		logger.Debug("WeatherController - Getting Weather.");

		var data = await weatherManager.GetWeather(lat, lon);

		logger.Debug(data?.SunriseTime?.ToString() ?? "data sunrise time is null.");

		if (data is null)
			return NoContent();

		return Ok(data);
	}

	[HttpGet("test/string")]
	[ProducesResponseType((int)HttpStatusCode.OK)]
	public async Task<IActionResult> GetTestString([FromQuery] string lat, [FromQuery] string lon)
	{
		var x = await weatherManager.GetWeatherTestString(lat, lon);

		if (x is null)
			return NoContent();

		return Ok(x);
	}
}