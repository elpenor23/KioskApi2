using Microsoft.AspNetCore.Mvc;
using KioskApi2.Models;
using KioskApi2.Managers;

namespace KioskApi2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClothingController(IConfiguration configuration, Serilog.ILogger logger, IClothingManager clothingManager) : ControllerBase
{
    private readonly IClothingManager _clothingManager = clothingManager;
    private readonly Serilog.ILogger _logger = logger;
    private readonly IConfiguration _configuration = configuration;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BodyPart>>> GetAllClothes()
    {
        _logger.Debug("ClothingController - GetAllClothes");
        var data = await _clothingManager.GetClothing();
        return Ok(data);
    }

    [HttpGet]
    [Route("BodyParts")]
    public async Task<ActionResult<IEnumerable<BodyPart>>> GetBodyParts()
    {
        _logger.Debug("ClothingController - GetBodyParts");
        var data = await _clothingManager.GetBodyParts();
        return Ok(data);
    }

    [HttpGet]
    [Route("Calculate")]
    public async Task<ActionResult<IEnumerable<PersonsClothing>>> GetClothingCalculated(
        [FromQuery] string feels,
        [FromQuery] string ids,
        [FromQuery] string names,
        [FromQuery] string colors,
        [FromQuery] string lat,
        [FromQuery] string lon)
    {
        _logger.Debug("ClothingController - GetClothingCalculated");
        IEnumerable<PersonsClothing> data;

        try
        {
            data = await _clothingManager.GetCalculatedClothing(feels, ids, names, colors, lat, lon);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Clothing Controller", ex.Message);
            return BadRequest(ModelState);
        }

        return Ok(data);
    }
}