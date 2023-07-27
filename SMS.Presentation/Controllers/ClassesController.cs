using SMS.Models.Helpers;

namespace SMS.Presentation.Controllers;

[Authorize(Policy = "Normal")]
[Route("api/[controller]")]
[ApiController]
public class ClassesController : ControllerBase
{
    private readonly IClassesService _classService;
    private readonly IUserClassService _userClassService;

    public ClassesController(IClassesService classService, IUserClassService userClassService)
    {
        _classService = classService;
        _userClassService = userClassService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<GetClassDto>> GetAll(int pageNumber, int pageSize)
    {
        return Ok(PaginatedList<GetClassDto>.Create(_classService.GetAll(),pageNumber,pageSize));
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

    [HttpPost("/assign-user")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<GetUserClassDto>> AssignUser(AddUserClassDto dto)
    {
        return Ok(await _userClassService.Add(dto));
    }
}
