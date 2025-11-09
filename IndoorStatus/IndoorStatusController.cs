using Microsoft.AspNetCore.Mvc;


namespace KioskApi2.IndoorStatus;

[Route("api/[controller]")]
[ApiController]
public class IndoorStatusController(IIndoorStatusManager indoorStatusManager, Serilog.ILogger logger) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IndoorStatusData>> Get()
	{
		logger.Debug("IndoorStatusController - Getting Indoor Status");

		var data = await indoorStatusManager.GetIndoorStatus();

		return Ok(data);

	}

	[HttpPost]
	public async Task<ActionResult<IndoorStatusData>> Post([FromBody] string status)
	{
		logger.Debug("IndoorStatusController - Posting Indoor Status");

		var data = await indoorStatusManager.SaveIndoorStatus(status);

		return Ok(data);
	}
}