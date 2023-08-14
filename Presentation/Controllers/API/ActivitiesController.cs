﻿namespace Presentation.Controllers.API;

[Route("api/activiteis")]
[ApiController]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityService _activityService;
    private readonly ILogger<ActivitiesController> _logger;

    public ActivitiesController(IActivityService activityService, ILogger<ActivitiesController> logger)
    {
        _activityService = activityService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll([FromHeader] int schoolId, int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_activityService.GetAll(pageNumber, pageSize, schoolId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromHeader] int schoolId, int id)
    {
        var dto = await _activityService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResponseHandler.BadRequest<string>("Not Found Activity"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromHeader] int schoolId, AddActivityDto dto)
    {
        return Ok(await _activityService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromHeader] int schoolId, int id, UpdateActivityDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _activityService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove([FromHeader] int schoolId, int id)
    {
        var result = await _activityService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}