namespace Presentation.Controllers.API;

[Authorize]
[Route("api/user-records")]
[ApiController]
[ApiExplorerSettings(GroupName = "Users")]
public class UserRecordsController(IUserRecordService userRecordService) : ControllerBase
{
    private readonly IUserRecordService _userRecordService = userRecordService;

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var result = _userRecordService.GetAll(pageNumber, pageSize);
        Response.AddPaginationHeader(result.Data);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _userRecordService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResultHandler.BadRequest<string>("Not Found UserRecord"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddUserRecordDto dto)
    {
        return Ok(await _userRecordService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserRecordDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _userRecordService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _userRecordService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
