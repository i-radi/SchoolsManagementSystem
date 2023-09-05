namespace Presentation.Controllers.API;

[Authorize]
[Route("api/activity-times")]
[ApiController]
public class ActivityTimesController : ControllerBase
{
    private readonly IActivityTimeService _activityTimeService;
    private readonly ILogger<ActivityTimesController> _logger;

    public ActivityTimesController(
        IActivityTimeService activityTimeService,
        ILogger<ActivityTimesController> logger)
    {
        _activityTimeService = activityTimeService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_activityTimeService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _activityTimeService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResponseHandler.BadRequest<string>("Not Found Activity Time"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddActivityTimeDto dto)
    {
        return Ok(await _activityTimeService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateActivityTimeDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _activityTimeService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _activityTimeService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
