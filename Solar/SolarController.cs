using Microsoft.AspNetCore.Mvc;

namespace KioskApi2.Solar;

[Route("api/[controller]")]
[ApiController]
public class SolarController(Serilog.ILogger logger, ISolarManager solarManager) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        logger.Debug("SolarController - Getting Solar Data.");

        try
        {
            var data = await solarManager.GetSolarData();

            if (data is null)
                return NoContent();

            return Ok(data);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Solar Controller", ex.Message);
            return BadRequest(ModelState);
        }


    }
}