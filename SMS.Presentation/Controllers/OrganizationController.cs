using Microsoft.AspNetCore.Mvc;
using SMS.Core.IServices;
using SMS.VModels.DTOS.Organizations.Commands;
using SMS.VModels.DTOS.Organizations.Queries;

namespace SMS.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _organizationService;

    public OrganizationController(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<GetClassDto>> GetAll()
    {
        return Ok(_organizationService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetClassDto>> GetById(int id)
    {
        var dto = await _organizationService.GetById(id);
        if (dto is null)
            return BadRequest("Not Found Organization");
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<GetClassDto>> Create(AddClassDto dto)
    {
        return Ok(await _organizationService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateClassDto dto)
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
