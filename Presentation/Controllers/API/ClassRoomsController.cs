namespace Presentation.Controllers.API;

[Authorize]
[Route("api/classrooms")]
[ApiController]

[ApiExplorerSettings(GroupName = "Classes")]
public class ClassroomsController(IClassroomService classroomService) : ControllerBase
{
    private readonly IClassroomService _classroomService = classroomService;

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var result = _classroomService.GetAll(pageNumber, pageSize);
        Response.AddPaginationHeader(result.Data);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _classroomService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResultHandler.BadRequest<string>("Not Found Class"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddClassroomDto dto)
    {
        return Ok(await _classroomService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateClassroomDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _classroomService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _classroomService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
