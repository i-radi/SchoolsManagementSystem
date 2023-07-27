using SMS.Models.Helpers;

namespace SMS.Presentation.Controllers;

[Authorize(Policy = "Normal")]
[Route("api/[controller]")]
[ApiController]
public class SchoolsController : ControllerBase
{
    private readonly ISchoolService _schoolService;

    public SchoolsController(ISchoolService schoolService)
    {
        _schoolService = schoolService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<GetSchoolDto>> GetAll(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(PaginatedList<GetSchoolDto>.Create(_schoolService.GetAll(), pageNumber, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetSchoolDto>> GetById(int id)
    {
        var dto = await _schoolService.GetById(id);
        if (dto is null)
            return BadRequest("Not Found School");
        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<GetSchoolDto>> Create(AddSchoolDto dto)
    {
        return Ok(await _schoolService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateSchoolDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("Id not matched");
        }
        if (!await _schoolService.Update(dto))
        {
            return BadRequest("Not Updated");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _schoolService.Delete(id))
        {
            return BadRequest("Not Deleted");
        }
        return NoContent();
    }
}
