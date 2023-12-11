namespace Presentation.Controllers.API;

[Authorize]
[Route("api/record-classes")]
[ApiController]
[ApiExplorerSettings(GroupName = "Records")]
public class RecordClassesController : ControllerBase
{
    private readonly IRecordClassService _recordClassService;

    public RecordClassesController(IRecordClassService recordClassService)
    {
        _recordClassService = recordClassService;
    }

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_recordClassService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _recordClassService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResultHandler.BadRequest<string>("Not Found RecordClass"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddRecordClassDto dto)
    {
        return Ok(await _recordClassService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateRecordClassDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _recordClassService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _recordClassService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
