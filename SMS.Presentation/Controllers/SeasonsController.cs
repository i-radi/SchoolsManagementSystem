using SMS.Models.Helpers;

namespace SMS.Presentation.Controllers;

[Authorize(Policy = "Normal")]
[Route("api/[controller]")]
[ApiController]
public class SeasonsController : ControllerBase
{
    private readonly ISeasonService _seasonService;

    public SeasonsController(ISeasonService seasonService)
    {
        _seasonService = seasonService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<GetSeasonDto>> GetAll(int pageNumber, int pageSize)
    {
        return Ok(PaginatedList<GetSeasonDto>.Create(_seasonService.GetAll(), pageNumber,pageSize));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetSeasonDto>> GetById(int id)
    {
        var dto = await _seasonService.GetById(id);
        if (dto is null)
            return BadRequest("Not Found Season");
        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<GetSeasonDto>> Create(AddSeasonDto dto)
    {
        return Ok(await _seasonService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateSeasonDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("Id not matched");
        }
        if (!await _seasonService.Update(dto))
        {
            return BadRequest("Not Updated");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _seasonService.Delete(id))
        {
            return BadRequest("Not Deleted");
        }
        return NoContent();
    }
}
