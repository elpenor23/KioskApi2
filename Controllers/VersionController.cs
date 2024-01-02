using Microsoft.AspNetCore.Mvc;

namespace KioskApi2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VersionController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Version>> Get()
    {
        var v = new Version
        {
            version = "v1.1"
        };

        return Ok(v);
    }
}
public class Version
{
    public string? version { get; set; }
}
