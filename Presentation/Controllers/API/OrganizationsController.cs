namespace Presentation.Controllers.API;

[Authorize]
[Route("api/organizations")]
[ApiController]
[ApiExplorerSettings(GroupName = "Organizations")]
public class OrganizationsController(IOrganizationService organizationService) : ControllerBase
{
    private readonly IOrganizationService _organizationService = organizationService;

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var result = _organizationService.GetAll(pageNumber, pageSize);
        Response.AddPaginationHeader(result.Data);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _organizationService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResultHandler.BadRequest<string>("Not Found Organization"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddOrganizationDto dto) 
    {
        return Ok(await _organizationService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateOrganizationDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _organizationService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _organizationService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
