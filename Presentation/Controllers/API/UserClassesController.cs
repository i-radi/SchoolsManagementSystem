using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.API;

[Authorize]
[Route("api/user-classes")]
[ApiController]
[ApiExplorerSettings(GroupName = "Users")]
public class UserClassesController(IUserClassService userClassService , ISeasonService seasonService ,  IClassroomService classroomService , UserManager<User> userManager , IUserTypeService userTypeService) : ControllerBase
{
    private readonly IUserClassService _userClassService = userClassService;
    private readonly ISeasonService _seasonService = seasonService;
    private readonly IClassroomService _classroomService = classroomService;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IUserTypeService _userTypeService = userTypeService;

    [HttpGet]
    public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var result = _userClassService.GetAll(pageNumber, pageSize);
        Response.AddPaginationHeader(result.Data);
        return Ok(result);
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

    [ApiExplorerSettings(GroupName = "V2")]
    [SwaggerOperation(Tags = new[] { "Classes" })]
    [HttpPost("assign")]
    public async Task<IActionResult> Add(AddUserClassDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user == null) return BadRequest(ResultHandler.BadRequest<string>("User Is Not Exist"));
        var classroom =await _classroomService.GetById(dto.ClassroomId); 
         if(classroom==null) return BadRequest(ResultHandler.BadRequest<string>("Classroom Is Not Exist"));
        var seasson = await _seasonService.GetById(dto.SeasonId); 
        if(seasson == null) return BadRequest(ResultHandler.BadRequest<string>("Season Is Not Exist"));
        var usertype = await _userTypeService.GetById(dto.UserTypeId);
        if (usertype == null) return BadRequest(ResultHandler.BadRequest<string>("User Type Is Not Exist"));
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

    [ApiExplorerSettings(GroupName = "V2")]
    [SwaggerOperation(Tags = new[] { "Classes" })]
    [HttpDelete("unassign")]
    public async Task<IActionResult> Remove(AddUserClassDto dto)
    {
        var result = await _userClassService.Delete(dto);
        if (!result.Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
        }
        return Ok(result);
    }
}
