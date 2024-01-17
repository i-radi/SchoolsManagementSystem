using Humanizer;
using Microsoft.AspNetCore.Identity;
using Persistance.Repos;
using Swashbuckle.AspNetCore.Annotations;
using System.IO;
using VModels.DTOS.Users.Commands;

namespace Presentation.Controllers.API;

[Authorize]
[Route("api/users")]
[ApiController]
[ApiExplorerSettings(GroupName = "Users")]
public class UsersController(
    UserManager<User> userManager,
    RoleManager<Role> roleManager ,
    IUserTypeService userTypeService,
    IMapper mapper,
    IWebHostEnvironment webHostEnvironment,
    IExportService<GetUserDto> exportService,
    BaseSettings baseSettings,
    IAuthService authService,
    IAttachmentService attachmentService ,
    SharedSettings sharedSettings
    ) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly IUserTypeService _userTypeService = userTypeService;
    private readonly IMapper _mapper = mapper;
    private readonly IExportService<GetUserDto> _exportService = exportService;
    private readonly BaseSettings _baseSettings = baseSettings;
    private readonly IAuthService _authService = authService;
    private readonly IAttachmentService _attachmentService = attachmentService;
    private readonly SharedSettings _sharedSettings = sharedSettings;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    [HttpPost()]
    public async Task<IActionResult> AddAsync(AddUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.AddAsync(dto);

        return Ok(result);
    }

    [ApiExplorerSettings(GroupName = "V2")]
    [SwaggerOperation(Tags = new[] { "User" })]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var modelItem = await _userManager.Users
            .Include(u => u.UserRoles) 
            .ThenInclude(u=>u.Role)
            .Include(u=>u.UserOrganizations)
            .ThenInclude(u=>u.Organization)       
            .FirstOrDefaultAsync(u => u.Id == id);

        if (!string.IsNullOrEmpty(modelItem.ProfilePicturePath))
        {
            modelItem.ProfilePicturePath = $"{_baseSettings.url}/{_baseSettings.usersPath}/{modelItem.ProfilePicturePath}";
        }

        var result = _mapper.Map<GetUserDto>(modelItem);
        return Ok(ResultHandler.Success(result));
    }

    [ApiExplorerSettings(GroupName = "V2")]
    [SwaggerOperation(Tags = new[] { "User" })]
    [HttpGet("profile/{id}")]
    public async Task<IActionResult> GetProfileById(int id)
    {
        var modelItem = await _userManager.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Organization)
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.School)
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Activity)
            .Include(x => x.UserOrganizations)
            .ThenInclude(x => x.Organization)
            .AsSplitQuery() 
            .FirstOrDefaultAsync(u => u.Id == id);
            
        if (!string.IsNullOrEmpty(modelItem.ProfilePicturePath))
        {
            modelItem.ProfilePicturePath = $"{_baseSettings.url}/{_baseSettings.usersPath}/{modelItem.ProfilePicturePath}";
        }

        var result = modelItem.ToGetProfileDto();
        
        return Ok(ResultHandler.Success(result));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditUserById(int id, UpdateUserDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ResultHandler.BadRequest<string>("Invalid User Id."));
        }

        var modelItem = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (modelItem is null)
        {
            return NotFound(ResultHandler.NotFound<string>("User Not Found."));
        }

        modelItem = modelItem.MapUpdateUserDto(dto);
        var result = await _userManager.UpdateAsync(modelItem);
        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description},";

            return NotFound(ResultHandler.BadRequest<string>(errors));
        }

        var userDto = _mapper.Map<GetProfileDto>(modelItem);
        return Ok(ResultHandler.Success(userDto));
    }

    [ApiExplorerSettings(GroupName = "V2")]
    [SwaggerOperation(Tags = new[] { "User" })]
    [HttpPut("change-image/{id}")]
    public async Task<IActionResult> ChangeImage(int id, IFormFile image)
    {
        var modelItem = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

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

        if(modelItem.ProfilePicturePath != null && modelItem.ProfilePicturePath != _sharedSettings.DefaultProfileImage)
        {      
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, _baseSettings.usersPath,modelItem.ProfilePicturePath);
            
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var modelItem = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (modelItem is null)
        {
            return NotFound(ResultHandler.NotFound<string>("User Not Found."));
        }
        var result = await _userManager.DeleteAsync(modelItem);
        if (!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
                errors += $"{error.Description},";

            return NotFound(ResultHandler.BadRequest<string>(errors));
        }
        return Ok(ResultHandler.BadRequest<bool>("deleted successfully"));
    }

    [HttpGet("organization/{organizationId}")]
    public async Task<IActionResult> GetByOrganization([FromRoute] int organizationId, int pageNumber = 1, int pageSize = 10)
    {
        var usersQuery = _userManager.Users.Include(u => u.UserOrganizations).AsQueryable();

        if (organizationId > 0)
        {
            usersQuery = usersQuery.Where(u => u.UserOrganizations.Any(uo => uo.OrganizationId == organizationId));
        }

        var modelItems = await usersQuery.ToPaginatedListAsync(pageNumber, pageSize);

        foreach (var user in modelItems)
        {
            if (!string.IsNullOrEmpty(user.ProfilePicturePath))
            {
                user.ProfilePicturePath = Path.Combine(
                _baseSettings.url,
                _baseSettings.usersPath,
                user.ProfilePicturePath);
            }
        }
        Response.AddPaginationHeader(modelItems);
        return Ok(ResultHandler.Success(modelItems));
    }

    [HttpGet("organization/{organizationId}/search")]
    public async Task<IActionResult> SearchByNameOrMobile([FromRoute] int organizationId, string nameOrMobile)
    {
        var usersQuery = _userManager.Users.Include(u => u.UserOrganizations).AsQueryable();

        if (organizationId > 0)
        {
            usersQuery = usersQuery.Where(u => u.UserOrganizations.Any(uo => uo.OrganizationId == organizationId));
        }

        if (!string.IsNullOrEmpty(nameOrMobile))
        {
            usersQuery = usersQuery
                .Where(u => u.UserName!.Contains(nameOrMobile)
            || u.Name.Contains(nameOrMobile)
            || u.Email!.Contains(nameOrMobile)
            || u.PhoneNumber == nameOrMobile);
        }
        var modelItems = await usersQuery.ToListAsync();

        foreach (var user in modelItems)
        {
            if (!string.IsNullOrEmpty(user.ProfilePicturePath))
            {
                user.ProfilePicturePath = $"{_baseSettings.url}/{_baseSettings.usersPath}/{user.ProfilePicturePath}";
            }
        }

        var result = _mapper.Map<IEnumerable<GetUserDto>>(modelItems);

        return Ok(ResultHandler.Success(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var modelItems = await _userManager.Users
        .Include(u => u.UserRoles)
            .Select(u => _mapper.Map<GetUserDto>(u))
            .ToPaginatedListAsync(pageNumber, pageSize);

        foreach (var user in modelItems)
        {
            if (!string.IsNullOrEmpty(user.ProfilePicturePath))
            {
                user.ProfilePicturePath = $"{_baseSettings.url}/{_baseSettings.usersPath}/{user.ProfilePicturePath}";
            }
        }
        Response.AddPaginationHeader(modelItems);
        return Ok(ResultHandler.Success(modelItems));
    }

    [HttpGet("export-users")]
    public async Task<IActionResult> ExportUsers(
        int pageNumber = 1,
        int pageSize = 10,
        int? organizationId = null,
        bool isFileUrlRequired = false)
    {
        var users = _userManager.Users
            .Include(u => u.UserRoles)
            .Include(u => u.UserOrganizations)
            .AsQueryable();

        if (organizationId is not null)
        {
            users = users.Where(u => u.UserOrganizations.Any(uo => uo.OrganizationId == organizationId));
        }

        var modelItems = PaginatedList<User>.Create(await users.ToListAsync(), pageNumber, pageSize);

        foreach (var user in modelItems)
        {
            if (!string.IsNullOrEmpty(user.ProfilePicturePath))
            {
                user.ProfilePicturePath = Path.Combine(
                _baseSettings.url,
                _baseSettings.usersPath,
                user.ProfilePicturePath);
            }
        }

        var data = _mapper.Map<IEnumerable<GetUserDto>>(modelItems).ToList();

        if (isFileUrlRequired)
        {
            var fileName = $"users_export_{Guid.NewGuid()}.xlsx";
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "export attachments", fileName);

            filePath = await _exportService.ExportToExcelAndSave(data, filePath);

            var fileUrl = $"{Request.Scheme}://{Request.Host}/export attachments/{fileName}";

            return Ok(ResultHandler.Success(fileUrl));
        }

        var result = await _exportService.ExportToExcel(data);

        return Ok(ResultHandler.Success(result));
    }

    [ApiExplorerSettings(GroupName = "V2")]
    [SwaggerOperation(Tags = new[] { "User" })]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePasswordByIdAsync(ChangeUserPasswordByIdDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.ChangeUserPasswordByIdAsync(dto);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

    [ApiExplorerSettings(GroupName = "V2")]
    [SwaggerOperation(Tags = new[] { "User" })]
    [HttpPost("assign-user-to-organization")]
    public async Task<IActionResult> AddUserToOrganizations(AddUserToOrganizationsDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.AddUserToOrganizationAsync(dto);

        return Ok(result);
    }

    [ApiExplorerSettings(GroupName = "V2")]
    [SwaggerOperation(Tags = new[] { "User" })]
    [HttpGet("user-types")]
    public IActionResult GetUsersTypes()
    {
        var result = _userTypeService.GetAll();
        return Ok(result);
    }
}

