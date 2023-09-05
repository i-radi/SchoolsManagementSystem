namespace Presentation.Controllers.API;

[Authorize]
[Route("api/classrooms")]
[ApiController]
public class ClassroomsController : ControllerBase
{
    private readonly IClassroomService _classroomService;
    private readonly IUserClassService _userClassService;
    private readonly ILogger<ClassroomsController> _logger;

    public ClassroomsController(IClassroomService classroomService, IUserClassService userClassService, ILogger<ClassroomsController> logger)
    {
        _classroomService = classroomService;
        _userClassService = userClassService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_classroomService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _classroomService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResponseHandler.BadRequest<string>("Not Found Class"));
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
            return BadRequest(ResponseHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _classroomService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _classroomService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }

    [HttpPost("add-class-member")]
    public async Task<IActionResult> AssignUser(AddUserClassDto dto)
    {
        var result = await _userClassService.Add(dto);
        return Ok(await _userClassService.GetById(result.Data.Id));
    }
}
