using Microsoft.AspNetCore.Mvc;
using KioskApi2.Models;
using KioskApi2.Managers;

namespace KioskApi2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SolarController(IConfiguration configuration) : ControllerBase
{
    private readonly SolarManager solarManager = new(configuration);

    [HttpGet]
    public async Task<ActionResult<SolarData>> Get()
    {        
        SolarData data;
        try
        {
            data = await solarManager.GetSolarData();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Solar Controller", ex.Message);
            return BadRequest(ModelState);
        }
        return data;
    }
}