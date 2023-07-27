using SMS.Models.Helpers;

namespace SMS.Presentation.Controllers;

[Authorize(Policy = "Normal")]
[Route("api/[controller]")]
[ApiController]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<GetGradeDto>> GetAll(int pageNumber, int pageSize)
    {
        return Ok(PaginatedList<GetGradeDto>.Create(_gradeService.GetAll(), pageNumber, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetGradeDto>> GetById(int id)
    {
        var dto = await _gradeService.GetById(id);
        if (dto is null)
            return BadRequest("Not Found Grade");
        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<GetGradeDto>> Create(AddGradeDto dto)
    {
        return Ok(await _gradeService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateGradeDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("Id not matched");
        }
        if (!await _gradeService.Update(dto))
        {
            return BadRequest("Not Updated");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _gradeService.Delete(id))
        {
            return BadRequest("Not Deleted");
        }
        return NoContent();
    }
}
