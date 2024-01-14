namespace Presentation.Controllers.API;

[Authorize]
[Route("api/records")]
[ApiController]
[ApiExplorerSettings(GroupName = "Records")]
public class RecordsController(IRecordService recordService) : ControllerBase
{
    private readonly IRecordService _recordService = recordService;

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10, int schoolId = 0)
    {
        var result = _recordService.GetAll(pageNumber, pageSize);
        Response.AddPaginationHeader(result.Data);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _recordService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResultHandler.BadRequest<string>("Not Found Record"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddRecordDto dto)
    {
        return Ok(await _recordService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateRecordDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _recordService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _recordService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
