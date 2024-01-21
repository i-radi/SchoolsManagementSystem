namespace Presentation.Controllers.API;

[Authorize]
[Route("api/activity-instance-users")]
[ApiController]
[ApiExplorerSettings(GroupName = "Activities")]
public class ActivityInstanceUsersController(
    IActivityInstanceUserService activityInstanceUserService,
    UserManager<User> userManager,
      IActivityInstanceService activityInstanceService) : ControllerBase
{
    private readonly IActivityInstanceUserService _activityInstanceUserService = activityInstanceUserService;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IActivityInstanceService _activityInstanceService = activityInstanceService;

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var result = _activityInstanceUserService.GetAll(pageNumber, pageSize);
        Response.AddPaginationHeader(result.Data);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _activityInstanceUserService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResultHandler.BadRequest<string>("Not Found Activity InstanceUser"));
        return Ok(dto);
    }


    [HttpPost()]
    public async Task<IActionResult> Add(AddActivityInstanceUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user == null) return BadRequest(ResultHandler.BadRequest<string>("User Is Not Exist"));
        var activityIntance = _activityInstanceService.GetById(dto.ActivityInstanceId);
        if (activityIntance == null) return BadRequest(ResultHandler.BadRequest<string>("Activity Instance  Is Not Exist"));

        return Ok(await _activityInstanceUserService.Add(dto));
    }


    [HttpPut("{activityinstanceuserid}")]
    public async Task<IActionResult> Update(int id, UpdateActivityInstanceUserDto dto)
    {
        if (id != dto.Id) return BadRequest(ResultHandler.BadRequest<string>("Id not matched"));

        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user == null) return BadRequest(ResultHandler.BadRequest<string>("User Is Not Exist"));
        var activityIntance = _activityInstanceService.GetById(dto.ActivityInstanceId);
        if (activityIntance == null) return BadRequest(ResultHandler.BadRequest<string>("Activity Instance  Is Not Exist"));

        var result = await _activityInstanceUserService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }


    [HttpDelete()]
    public async Task<IActionResult> Remove(int activityinstanceid, int userid)
    {
        var user = await _userManager.FindByIdAsync(userid.ToString());
        if (user == null) return BadRequest(ResultHandler.BadRequest<string>("User Is Not Exist"));
        var activityIntance = _activityInstanceService.GetById(activityinstanceid);
        if (activityIntance == null) return BadRequest(ResultHandler.BadRequest<string>("Activity Instance  Is Not Exist"));

        var result = await _activityInstanceUserService.Delete(userid, activityinstanceid);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
