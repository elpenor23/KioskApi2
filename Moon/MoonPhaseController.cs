using Microsoft.AspNetCore.Mvc;

namespace KioskApi2.Moon;

[Route("api/[controller]")]
[ApiController]
public class MoonPhaseController(Serilog.ILogger logger, IMoonPhaseManager moonPhaseManager) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetMoonPhase([FromQuery] string lat, [FromQuery] string lon)
	{
		logger.Debug("ITS WORKING!!");

		var data = await moonPhaseManager.GetMoonPhase(lat, lon);

		return Ok(data);

	}
}