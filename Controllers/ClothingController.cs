using Microsoft.AspNetCore.Mvc;
using KioskApi2.Models;
using KioskApi2.Managers;

namespace KioskApi2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClothingController(IConfiguration configuration) : ControllerBase
{
    private readonly ClothingManager clothingManager = new(configuration);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BodyPart>>> GetAllClothes()
    {
        var data = await clothingManager.GetClothing();
        return Ok(data);
    }

    [HttpGet]
    [Route("BodyParts")]
    public async Task<ActionResult<IEnumerable<BodyPart>>> GetBodyParts()
    {
        var data = await clothingManager.GetBodyParts();
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
        IEnumerable<PersonsClothing> data;

        try
        {
            data = await clothingManager.GetCalculatedClothing(feels, ids, names, colors, lat, lon);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Clothing Controller", ex.Message);
            return BadRequest(ModelState);
        }

        return Ok(data);
    }
}