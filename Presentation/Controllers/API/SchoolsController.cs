namespace Presentation.Controllers.API;

//[Authorize]
[Route("api/schools")]
[ApiController]
[ApiExplorerSettings(GroupName = "Schools")]
public class SchoolsController : ControllerBase
{
    private readonly ISchoolService _schoolService;
    private readonly ISeasonService _seasonService;
    private readonly IOrganizationService _organizationService;

    public SchoolsController(ISchoolService schoolService, ISeasonService seasonService, IOrganizationService organizationService)
    {
        _schoolService = schoolService;
        _seasonService = seasonService;
        _organizationService = organizationService;
    }

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_schoolService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("{schoolId}/season/{seasonId}")]
    public async Task<IActionResult> GetSchoolReport([FromRoute] int schoolId, [FromRoute] int seasonId)
    {
        var schooldto = await _schoolService.GetById(schoolId);
        if (schooldto is null)
            return BadRequest(ResponseHandler.BadRequest<string>("Not Found School"));

        var seasondto = await _seasonService.GetById(seasonId);
        if (seasondto is null)
            return BadRequest(ResponseHandler.BadRequest<string>("Not Found Season"));

        return Ok(await _schoolService.GetSchoolReport(schoolId, seasonId));
    }


    [HttpGet("organization/{organizationId}")]
    public async Task<IActionResult> GetByOrganization([FromRoute] int organizationId, int pageNumber = 1, int pageSize = 10)
    {
        if (organizationId > 0)
        {
            var orgResponse = (await _organizationService.GetById(organizationId));
            if (orgResponse is null)
            {
                return BadRequest(ResponseHandler.BadRequest<string>("Invalid Organization Id."));
            }
            return Ok(_schoolService.GetByOrganization(organizationId, pageNumber, pageSize));
        }
        return Ok(_schoolService.GetAll(pageNumber, pageSize));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _schoolService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResponseHandler.BadRequest<string>("Not Found School"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddSchoolDto dto)
    {
        return Ok(await _schoolService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateSchoolDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _schoolService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _schoolService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
