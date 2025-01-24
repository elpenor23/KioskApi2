using Microsoft.AspNetCore.Mvc;

namespace KioskApi2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VersionController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Version>> Get()
    {
        var v = await Task.Run(() =>
            new Version
            {
                version = "v2.0"
            });


        return Ok(v);
    }
}
public class Version
{
    public string? version { get; set; }
}
