using Microsoft.AspNetCore.Mvc;

namespace KioskApi2.Clothing;

[Route("api/[controller]")]
[ApiController]
public class ClothingController(Serilog.ILogger logger, IClothingManager clothingManager) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllClothes()
    {
        logger.Debug("ClothingController - GetAllClothes");

        var data = clothingManager.GetClothing();

        return Ok(data);
    }

    [HttpGet]
    [Route("BodyParts")]
    public IActionResult GetBodyParts()
    {
        logger.Debug("ClothingController - GetBodyParts");
        var data = clothingManager.GetBodyParts();
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
        logger.Debug("ClothingController - GetClothingCalculated");

        var data = await clothingManager.GetCalculatedClothing(feels, ids, names, colors, lat, lon);

        return Ok(data);
    }
}