using Microsoft.AspNetCore.Mvc;
using KioskApi.Models;
using KioskApi.Managers;

namespace KioskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IndoorStatusController : ControllerBase
{
    private IndoorStatusManager indoorStatusManager;
    public IndoorStatusController( IConfiguration configuration)
    {
        indoorStatusManager = new IndoorStatusManager(configuration);
    }

    [HttpGet]
    public async Task<ActionResult<IndoorStatusData>> Get()
    {
        var data = await indoorStatusManager.GetIndoorStatus();
        
        return Ok(data);

    }

    [HttpPost]
    public async Task<ActionResult<IndoorStatusData>> Post([FromBody]string status)
    {
        try{
        var data = await indoorStatusManager.SaveIndoorStatus(status);
        }catch (Exception ex)
        {
            ModelState.AddModelError("Indoor Status Controller", ex.Message);
            return BadRequest(ModelState);
        }

        return Ok();
    }
}