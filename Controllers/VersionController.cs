using Microsoft.AspNetCore.Mvc;

namespace KioskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VersionController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Version>> Get()
    {        
        var v = new Version();
        v.version = "v1.0";

        return Ok(v);
    }
}
public class Version
{
    public string? version { get; set; }
}
