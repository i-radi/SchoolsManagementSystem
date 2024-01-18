using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.API;

[Authorize]
[Route("api/activity-instances")]
[ApiController]
[ApiExplorerSettings(GroupName = "Activities")]
public class ActivityInstancesController(
    IActivityInstanceService activityInstanceService ,
    IActivityService activityService) : ControllerBase
{
    private readonly IActivityInstanceService _activityInstanceService = activityInstanceService;
    private readonly IActivityService _activityService = activityService;

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var result = _activityInstanceService.GetAll(pageNumber, pageSize);
        Response.AddPaginationHeader(result.Data);
        return Ok(result);
    }

    [ApiExplorerSettings(GroupName = "V2")]
    [SwaggerOperation(Tags = new[] { "Activities" })]
    [HttpGet("{activityId}")]
    public IActionResult GetByActivity(int activityId, int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_activityInstanceService.GetAll(pageNumber, pageSize, activityId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _activityInstanceService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResultHandler.BadRequest<string>("Not Found Activity Instance"));
        return Ok(dto);
    }

    [ApiExplorerSettings(GroupName = "V2")]
    [SwaggerOperation(Tags = new[] { "Activities" })]
    [HttpPost]
    public async Task<IActionResult> Add(AddActivityInstanceDto dto)
    {
        var activity =await _activityService.GetById(dto.ActivityId);
         if(activity is null)  return BadRequest(ResultHandler.BadRequest<string>("Activity Is Not Exist"));
        return Ok(await _activityInstanceService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateActivityInstanceDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _activityInstanceService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _activityInstanceService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
