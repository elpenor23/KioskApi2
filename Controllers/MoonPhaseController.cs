using Microsoft.AspNetCore.Mvc;
using KioskApi2.Models;
using KioskApi2.Managers;

namespace KioskApi2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoonPhaseController : ControllerBase
{
    private MoonPhaseManager moonPhaseManager;
    public MoonPhaseController(IConfiguration configuration)
    {
        moonPhaseManager = new MoonPhaseManager(configuration);
    }

    [HttpGet]
    public async Task<ActionResult<MoonData>> GetMoonPhase([FromQuery] string lat, [FromQuery] string lon)
    {
        MoonData data;
        try
        {
            data = await moonPhaseManager.GetMoonPhase(lat, lon);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Moon Controller", ex.Message);
            return BadRequest(ModelState);
        }
        return data;

    }
}