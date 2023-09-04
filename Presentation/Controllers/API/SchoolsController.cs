using Microsoft.AspNetCore.Identity;

namespace Presentation.Controllers.API;

//[Authorize]
[Route("api/schools")]
[ApiController]
public class SchoolsController : ControllerBase
{
    private readonly ISchoolService _schoolService;
    private readonly IOrganizationService _organizationService;

    public SchoolsController(ISchoolService schoolService,IOrganizationService organizationService)
    {
        _schoolService = schoolService;
        _organizationService = organizationService;
    }

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_schoolService.GetAll(pageNumber, pageSize));
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
    //[Authorize(Policy = "SuperAdmin")]
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
