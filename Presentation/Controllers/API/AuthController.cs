using Models.Results;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.API;

[Authorize]
[Route("api/auth")]
[ApiController]
[ApiExplorerSettings(GroupName = "Identity")]
public class AuthController(
    IAuthService authService,
    RoleManager<Role> roleManager,
    IMapper mapper,
    UserManager<User> userManager,
    IUserRoleService userRoleService,
    IAttachmentService attachmentService,
    IWebHostEnvironment webHostEnvironment,
    BaseSettings baseSettings) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IUserRoleService _userRoleService = userRoleService;
    private readonly IAttachmentService _attachmentService = attachmentService;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
    private readonly BaseSettings _baseSettings = baseSettings;

    [SwaggerOperation(Tags = new[] { "User Informations" })]
    [HttpGet("user-details/{id}")]
    public async Task<IActionResult> GetUserDetailsById(int id)
    {
        var modelItem = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (modelItem is null)
        {
            return NotFound(ResultHandler.NotFound<string>("User Not Found."));
        }

        if (!string.IsNullOrEmpty(modelItem.ProfilePicturePath))
        {
            modelItem.ProfilePicturePath = $"{_baseSettings.url}/{_baseSettings.usersPath}/{modelItem.ProfilePicturePath}";
        }

        var result = _mapper.Map<GetProfileDto>(modelItem);
        return Ok(ResultHandler.Success(result));
    }

    [SwaggerOperation(Tags = new[] { "User Informations" })]
    [HttpGet("user-roles-details/{userId}")]
    public async Task<IActionResult> GetUserRolesByUserId(int userId)
    {
        var modelItem = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (modelItem is null)
        {
            return NotFound(ResultHandler.NotFound<string>("User Not Found."));
        }

        var result = await _authService.GetUserRoles(userId);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [SwaggerOperation(Tags = new[] { "User Informations" })]
    [HttpGet("user-classes-details/{userId}")]
    public async Task<IActionResult> GetUserClassesByUserId(int userId)
    {
        var modelItem = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (modelItem is null)
        {
            return NotFound(ResultHandler.NotFound<string>("User Not Found."));
        }

        var result = await _authService.GetUserClassrooms(userId);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [AllowAnonymous]
    [SwaggerOperation(Tags = new[] { "Identity" })]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = new Result<JwtAuthResult>();

        if (Check.IsEmail(model.UserNameOrEmail))
        {
            result = await _authService.LoginAsync(model);
        }
        else
        {
            result = await _authService.LoginByUserNameAsync(model);
        }

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [AllowAnonymous]
    [SwaggerOperation(Tags = new[] { "Identity" })]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> GetRefreshTokenAsync(RefreshTokenInputDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RefreshTokenAsync(model);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [SwaggerOperation(Tags = new[] { "Identity" })]
    [HttpDelete("revoke-token")]
    public async Task<IActionResult> RevokeTokenAsync(string username)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RevokeTokenAsync(username);

        if (!result.Succeeded)
            return BadRequest("Opps, something went wrong.");

        return Ok("done.");
    }

    [SwaggerOperation(Tags = new[] { "Identity" })]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangeUserPasswordAsync(ChangeUserPasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.ChangeUserPasswordAsync(dto);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [SwaggerOperation(Tags = new[] { "Identity" })]
    [HttpPut("change-user-email-or-password/{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] ChangeUserDto dto)
    {
        if (id != dto.UserId)
            return BadRequest("Invalid User Id.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.UpdateAsync(dto);

        return Ok(result);
    }

    [SwaggerOperation(Tags = new[] { "Roles" })]
    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = (await _roleManager.Roles.ToListAsync())
            .Select(r => new
            {
                r.Id,
                r.Name
            });
        return Ok(roles);
    }

    [SwaggerOperation(Tags = new[] { "Roles" })]
    [HttpPost("user-roles")]
    public async Task<IActionResult> AddUserRoles(UserRoleRequest request)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
        if (user == null)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Invalid User Id."));
        }
        var dto = request.ToDto(request.UserId);
        if ((await _userRoleService.IsExists(dto)).Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("This role already exists."));
        }
        var result = await _userRoleService.Add(dto);
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [SwaggerOperation(Tags = new[] { "Roles" })]
    [HttpDelete("user-roles/{userRoleId}")]
    public async Task<IActionResult> DeleteUserRoles(int userRoleId)
    {
        var userRoleResponse = await _userRoleService.GetById(userRoleId);
        if (userRoleResponse is null || userRoleResponse.Data is null)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Invalid UserRole Id."));
        }
        var dto = new AddUserRoleDto
        {
            UserId = userRoleResponse.Data.UserId,
            RoleId = userRoleResponse.Data.RoleId,
            OrganizationId = userRoleResponse.Data.OrganizationId,
            SchoolId = userRoleResponse.Data.SchoolId,
            ActivityId = userRoleResponse.Data.ActivityId
        };
        var result = await _userRoleService.Delete(dto);
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [SwaggerOperation(Tags = new[] { "Roles" })]
    [HttpDelete("user-roles")]
    public async Task<IActionResult> DeleteUserRoles(UserRoleRequest request)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
        if (user == null)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Invalid User Id."));
        }
        var dto = request.ToDto(request.UserId);
        if (!(await _userRoleService.IsExists(dto)).Data)
        {
            return BadRequest(ResultHandler.BadRequest<string>("This role not exist."));
        }
        var result = await _userRoleService.Delete(dto);
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPut("change-image/{id}")]
    [SwaggerOperation(description: "maximum image size equals (5 MB) <br/> allowed formats: jpg, jpeg, png, gif.", Tags = new[] { "Identity" })]
    public async Task<IActionResult> UploadImage(int id, IFormFile image)
    {
        var modelItem = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        if (modelItem is null)
        {
            return NotFound(ResultHandler.NotFound<string>("not found user.."));
        }

        if (image is null || image.Length == 0)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Invalid image."));
        }

        if (image.Length > 5 * 1024 * 1024)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Image size exceeds the limit (5 MB)."));
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(image.FileName).ToLower();
        if (!allowedExtensions.Contains(fileExtension))
        {
            return BadRequest(ResultHandler.BadRequest<string>("Invalid file format. Allowed formats: jpg, jpeg, png, gif."));
        }

        modelItem.ProfilePicturePath = await _attachmentService.Upload(
            image,
            _webHostEnvironment,
            _baseSettings.usersPath,
            $"{modelItem.UserName}-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");

        var updatedModel = await _userManager.UpdateAsync(modelItem);

        if (!updatedModel.Succeeded)
        {
            return BadRequest(ResultHandler.BadRequest<string>(updatedModel.Errors.ToString()));
        }

        if (!string.IsNullOrEmpty(modelItem.ProfilePicturePath))
        {
            modelItem.ProfilePicturePath = $"{_baseSettings.url}/{_baseSettings.usersPath}/{modelItem.ProfilePicturePath}";
        }

        var result = _mapper.Map<GetUserDto>(modelItem);
        return Ok(ResultHandler.Success(result));
    }
}
