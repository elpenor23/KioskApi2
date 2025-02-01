using Microsoft.AspNetCore.Mvc;
using KioskApi2.Models;
using KioskApi2.Managers;

namespace KioskApi2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IndoorStatusController : ControllerBase
{
    private readonly IIndoorStatusManager _indoorStatusManager;
    private readonly IConfiguration _configuration;
    private readonly Serilog.ILogger _logger;
    public IndoorStatusController(IConfiguration configuration, IIndoorStatusManager indoorStatusManager, Serilog.ILogger logger)
    {
        _logger = logger;
        _configuration = configuration;
        _indoorStatusManager = indoorStatusManager;
    }

    [HttpGet]
    public async Task<ActionResult<IndoorStatusData>> Get()
    {
        _logger.Debug("IndoorStatusController - Getting Indoor Status");

        var data = await _indoorStatusManager.GetIndoorStatus();

        return Ok(data);

    }

    [HttpPost]
    public async Task<ActionResult<IndoorStatusData>> Post([FromBody] string status)
    {
        _logger.Debug("IndoorStatusController - Posting Indoor Status");
        try
        {
            var data = await _indoorStatusManager.SaveIndoorStatus(status);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Indoor Status Controller", ex.Message);
            return BadRequest(ModelState);
        }

        return Ok();
    }
}