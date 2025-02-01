using Microsoft.AspNetCore.Mvc;
using KioskApi2.Models;
using KioskApi2.Managers;

namespace KioskApi2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SolarController(IConfiguration configuration, Serilog.ILogger logger, ISolarManager solarManager) : ControllerBase
{
    private readonly ISolarManager _solarManager = solarManager;
    private readonly Serilog.ILogger _logger = logger;
    private readonly IConfiguration _configuration = configuration;

    [HttpGet]
    public async Task<ActionResult<SolarData>> Get()
    {
        _logger.Debug("SolarController - Getting Solar Data.");
        SolarData data;
        try
        {
            data = await _solarManager.GetSolarData();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Solar Controller", ex.Message);
            return BadRequest(ModelState);
        }
        return data;
    }
}