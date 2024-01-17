namespace Presentation.Controllers.API;

[Authorize]
[Route("api/activities")]
[ApiController]
[ApiExplorerSettings(GroupName = "Activities")]
public class ActivitiesController(IActivityService activityService) : ControllerBase
{
    private readonly IActivityService _activityService = activityService;

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var result = _activityService.GetAll(pageNumber, pageSize);
        Response.AddPaginationHeader(result.Data);
        return Ok(result);
    }

    [HttpGet("school/{schoolId}")]
    public IActionResult GetSchoolActivity(int schoolId, int pageNumber = 1, int pageSize = 10)
    {
        var result = _activityService.GetAll(pageNumber, pageSize, schoolId);
        Response.AddPaginationHeader(result.Data);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _activityService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResultHandler.BadRequest<string>("Not Found Activity"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddActivityDto dto)
    {
        return Ok(await _activityService.Add(dto));
    }

    [HttpPost("add activity instance to activity")]
    public async Task<ActionResult<GetActivityInstanceDto>> Add(AddActivityInstanceDto dto)
    {
      var instance =  await _activityService.AddActivityInstanceToActivity(dto);   
        return Ok(instance);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateActivityDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _activityService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _activityService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }

    [HttpGet("archive-activity/{id}")]
    public async Task<IActionResult> ArchiveActivityById(int id)
    {
        var dto = await _activityService.Archive(id);
        if (!dto.Succeeded)
        {
            return BadRequest(dto);
        }

        return Ok(dto);
    }
}
