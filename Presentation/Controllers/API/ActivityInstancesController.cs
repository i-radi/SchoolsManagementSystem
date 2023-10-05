namespace Presentation.Controllers.API;

[Authorize]
[Route("api/activity-instances")]
[ApiController]
[ApiExplorerSettings(GroupName = "Activities")]
public class ActivityInstancesController : ControllerBase
{
    private readonly IActivityInstanceService _activityInstanceService;
    private readonly ILogger<ActivityInstancesController> _logger;

    public ActivityInstancesController(
        IActivityInstanceService activityInstanceService,
        ILogger<ActivityInstancesController> logger)
    {
        _activityInstanceService = activityInstanceService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_activityInstanceService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("activity/{activityId}")]
    public IActionResult GetByActivity(int activityId, int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_activityInstanceService.GetAll(pageNumber, pageSize, activityId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _activityInstanceService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResponseHandler.BadRequest<string>("Not Found Activity Instance"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddActivityInstanceDto dto)
    {
        return Ok(await _activityInstanceService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateActivityInstanceDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _activityInstanceService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _activityInstanceService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
