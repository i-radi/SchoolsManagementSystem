namespace Presentation.Controllers.API;

[Authorize]
[Route("api/seasons")]
[ApiController]
[ApiExplorerSettings(GroupName = "Seasons")]
public class SeasonsController : ControllerBase
{
    private readonly ISeasonService _seasonService;

    public SeasonsController(ISeasonService seasonService)
    {
        _seasonService = seasonService;
    }

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_seasonService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _seasonService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResultHandler.BadRequest<string>("Not Found Season"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddSeasonDto dto)
    {
        return Ok(await _seasonService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateSeasonDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _seasonService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _seasonService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
