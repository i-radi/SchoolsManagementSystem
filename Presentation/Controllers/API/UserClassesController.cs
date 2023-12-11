using Microsoft.IdentityModel.Tokens;

namespace Presentation.Controllers.API;

[Authorize]
[Route("api/user-classes")]
[ApiController]
[ApiExplorerSettings(GroupName = "Users")]
public class UserClassesController : ControllerBase
{
    private readonly IUserClassService _userClassService;

    public UserClassesController(IUserClassService userClassService)
    {
        _userClassService = userClassService;
    }

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        return Ok(_userClassService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("by-user")]
    public IActionResult GetAllByUserId(int pageNumber = 1, int pageSize = 10, int userId = 0)
    {
        if (userId <= 0)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Invalid User Id."));
        }

        var dtos = _userClassService.GetAll(pageNumber, pageSize, userId);

        if (dtos.Data.IsNullOrEmpty())
            return BadRequest(ResultHandler.BadRequest<string>("Not Found UserClasses."));

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await _userClassService.GetById(id);
        if (dto.Data is null)
            return BadRequest(ResultHandler.BadRequest<string>("Not Found UserClass"));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddUserClassDto dto)
    {
        return Ok(await _userClassService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserClassDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Id not matched"));
        }
        var result = await _userClassService.Update(dto);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Updated"));
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await _userClassService.Delete(id);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
