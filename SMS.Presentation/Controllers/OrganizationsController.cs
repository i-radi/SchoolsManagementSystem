using Microsoft.AspNetCore.Authorization;

namespace SMS.Presentation.Controllers;

[Authorize(Policy = "Normal")]
[Route("api/[controller]")]
[ApiController]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationService _organizationService;

    public OrganizationsController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<GetOrganizationDto>> GetAll()
    {
        return Ok(_organizationService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetOrganizationDto>> GetById(int id)
    {
        var dto = await _organizationService.GetById(id);
        if (dto is null)
            return BadRequest("Not Found Organization");
        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<GetOrganizationDto>> Create(AddOrganizationDto dto)
    {
        return Ok(await _organizationService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateOrganizationDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("Id not matched");
        }
        if (!await _organizationService.Update(dto))
        {
            return BadRequest("Not Updated");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _organizationService.Delete(id))
        {
            return BadRequest("Not Deleted");
        }
        return NoContent();
    }
}
