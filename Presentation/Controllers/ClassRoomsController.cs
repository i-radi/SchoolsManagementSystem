namespace Presentation.Controllers;

[Authorize(Policy = "Normal")]
[Route("api/classrooms")]
[ApiController]
public class ClassRoomsController : ControllerBase
{
    private readonly IClassRoomService _classRoomService;
    private readonly IUserClassService _userClassService;

    public ClassRoomsController(IClassRoomService classRoomService, IUserClassService userClassService)
    {
        _classRoomService = classRoomService;
        _userClassService = userClassService;
    }

    [HttpGet]
    public IActionResult GetAll([FromHeader] int schoolId, int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_classRoomService.GetAll(pageNumber, pageSize, schoolId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromHeader] int schoolId, int id)
    {
        var dto = await _classRoomService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResponseHandler.BadRequest<string>("Not Found Class"));
        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Create([FromHeader] int schoolId, AddClassRoomDto dto)
    {
        return Ok(await _classRoomService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromHeader] int schoolId, int id, UpdateClassRoomDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _classRoomService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromHeader] int schoolId, int id)
    {
        var result = await _classRoomService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }

    [HttpPost("assign-user")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AssignUser([FromHeader] int schoolId, AddUserClassDto dto)
    {
        var result = await _userClassService.Add(dto);
        return Ok(await _userClassService.GetById(result.Data.Id));
    }
}
