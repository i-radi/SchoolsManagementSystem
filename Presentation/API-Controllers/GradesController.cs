namespace Presentation.API_Controllers;

[Authorize(Policy = "Normal")]
[Route("api/grades")]
[ApiController]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpGet]
    public IActionResult GetAll([FromHeader] int schoolId, int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_gradeService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromHeader] int schoolId, int id)
    {
        var dto = await _gradeService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResponseHandler.BadRequest<string>("Not Found Grade"));
        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Add([FromHeader] int schoolId, AddGradeDto dto)
    {
        return Ok(await _gradeService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromHeader] int schoolId, int id, UpdateGradeDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _gradeService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromHeader] int schoolId, int id)
    {
        var result = await _gradeService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
