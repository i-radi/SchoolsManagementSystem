namespace Presentation.Controllers;

[Authorize(Policy = "Normal")]
[Route("api/classes")]
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
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_classService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _classService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResponseHandler.BadRequest<string>("Not Found Class"));
        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Create(AddClassDto dto)
    {
        return Ok(await _classService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateClassDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _classService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _classService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }

    [HttpPost("assign-user")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AssignUser(AddUserClassDto dto)
    {
        var result = await _userClassService.Add(dto);
        return Ok(await _userClassService.GetById(result.Data.Id));
    }
}
