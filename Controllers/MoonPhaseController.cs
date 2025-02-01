using Microsoft.AspNetCore.Mvc;
using KioskApi2.Models;
using KioskApi2.Managers;
using Microsoft.Extensions.Logging;

namespace KioskApi2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoonPhaseController : ControllerBase
{
    private readonly IMoonPhaseManager _moonPhaseManager;
    private readonly Serilog.ILogger _logger;
    private readonly IConfiguration _configuration;
    public MoonPhaseController(IConfiguration configuration, Serilog.ILogger logger, IMoonPhaseManager moonPhaseManager)
    {
        _logger = logger;
        _moonPhaseManager = moonPhaseManager;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<MoonData>> GetMoonPhase([FromQuery] string lat, [FromQuery] string lon)
    {
        _logger.Debug("ITS WORKING!!");

        MoonData data;
        try
        {
            data = await _moonPhaseManager.GetMoonPhase(lat, lon);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Moon Controller", ex.Message);
            return BadRequest(ModelState);
        }
        return data;

    }
}