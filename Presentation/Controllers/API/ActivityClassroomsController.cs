namespace Presentation.Controllers.API;

[Authorize]
[Route("api/activity-classrooms")]
[ApiController]
[ApiExplorerSettings(GroupName = "Activities")]
public class ActivityClassroomsController : ControllerBase
{
    private readonly IActivityClassroomService _activityClassroomService;

    public ActivityClassroomsController(
        IActivityClassroomService activityClassroomService)
    {
        _activityClassroomService = activityClassroomService;
    }

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_activityClassroomService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _activityClassroomService.GetById(id);
        if (dto?.Data is null)
            return BadRequest(ResultHandler.BadRequest<string>("Not Found Activity Classroom"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddActivityClassroomDto dto)
    {
        return Ok(await _activityClassroomService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateActivityClassroomDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _activityClassroomService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _activityClassroomService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
