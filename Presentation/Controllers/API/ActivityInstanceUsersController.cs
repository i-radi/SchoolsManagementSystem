using Core.Services;

namespace Presentation.Controllers.API;

[Route("api/activity-instance-users")]
[ApiController]
public class ActivityInstanceUsersController : ControllerBase
{
    private readonly IActivityInstanceUserService _activityInstanceUserService;
    private readonly ILogger<ActivityInstanceUsersController> _logger;

    public ActivityInstanceUsersController(
        IActivityInstanceUserService activityInstanceUserService,
        ILogger<ActivityInstanceUsersController> logger)
    {
        _activityInstanceUserService = activityInstanceUserService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_activityInstanceUserService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _activityInstanceUserService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResponseHandler.BadRequest<string>("Not Found Activity InstanceUser"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddActivityInstanceUserDto dto)
    {
        return Ok(await _activityInstanceUserService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateActivityInstanceUserDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _activityInstanceUserService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _activityInstanceUserService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
