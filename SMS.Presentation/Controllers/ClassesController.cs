namespace SMS.Presentation.Controllers;

[Authorize(Policy = "Normal")]
[Route("api/[controller]")]
[ApiController]
public class ClassesController : ControllerBase
{
    private readonly IClassesService _classService;

    public ClassesController(IClassesService classService)
    {
        _classService = classService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<GetClassDto>> GetAll()
    {
        return Ok(_classService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetClassDto>> GetById(int id)
    {
        var dto = await _classService.GetById(id);
        if (dto is null)
            return BadRequest("Not Found Class");
        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<GetClassDto>> Create(AddClassDto dto)
    {
        return Ok(await _classService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateClassDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("Id not matched");
        }
        if (!await _classService.Update(dto))
        {
            return BadRequest("Not Updated");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _classService.Delete(id))
        {
            return BadRequest("Not Deleted");
        }
        return NoContent();
    }
}
