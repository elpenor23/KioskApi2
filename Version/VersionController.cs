using Microsoft.AspNetCore.Mvc;

namespace KioskApi2.Version;

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
                version = "v3.0"
            });


        return Ok(v);
    }
}
public class Version
{
    public string? version { get; set; }
}
