using Microsoft.AspNetCore.Hosting;
using Models.Entities.Identity;

namespace Presentation.Controllers.API;

[Authorize]
[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IExportService<GetUserDto> _exportService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string _imagesBaseURL;

    public UsersController(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment,
        IExportService<GetUserDto> exportService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _exportService = exportService;
        _webHostEnvironment = webHostEnvironment;
        _imagesBaseURL = webHostEnvironment.WebRootPath;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var modelItem = await _userManager.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (!string.IsNullOrEmpty(modelItem.ProfilePicturePath))
        {
            modelItem.ProfilePicturePath = Path.Combine(_imagesBaseURL, modelItem.ProfilePicturePath);
        }

        var result = _mapper.Map<GetUserDto>(modelItem);
        return Ok(ResponseHandler.Success(result));
    }

    [HttpGet("organization/{organizationId}")]
    public async Task<IActionResult> GetByOrganization([FromRoute]int organizationId ,int pageNumber = 1, int pageSize = 10)
    {
        var usersQuery = _userManager.Users.Include(u => u.UserOrganizations).AsQueryable();

        if (organizationId > 0)
        {
            usersQuery = usersQuery.Where(u => u.UserOrganizations.Any(uo => uo.OrganizationId == organizationId));
        }

        var modelItems = PaginatedList<User>.Create(await usersQuery.ToListAsync(), pageNumber, pageSize);

        foreach (var user in modelItems)
        {
            if (!string.IsNullOrEmpty(user.ProfilePicturePath))
            {
                user.ProfilePicturePath = Path.Combine(_imagesBaseURL, user.ProfilePicturePath);
            }
        }

        var result = _mapper.Map<IEnumerable<GetUserDto>>(modelItems);

        return Ok(ResponseHandler.Success(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var modelItems = PaginatedList<User>
            .Create(await _userManager.Users
            .Include(u => u.UserRoles)
            .ToListAsync(), pageNumber, pageSize);

        foreach (var user in modelItems)
        {
            if (!string.IsNullOrEmpty(user.ProfilePicturePath))
            {
                user.ProfilePicturePath = Path.Combine(_imagesBaseURL, user.ProfilePicturePath);
            }
        }

        var result = _mapper.Map<IEnumerable<GetUserDto>>(modelItems);

        return Ok(ResponseHandler.Success(result));
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
                user.ProfilePicturePath = Path.Combine(_imagesBaseURL, user.ProfilePicturePath);
            }
        }

        var data = _mapper.Map<IEnumerable<GetUserDto>>(modelItems).ToList();

        if (isFileUrlRequired)
        {
            var fileName = $"users_export_{Guid.NewGuid()}.xlsx";
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "export attachments", fileName);

            filePath = await _exportService.ExportToExcelAndSave(data, filePath);

            var fileUrl = $"{Request.Scheme}://{Request.Host}/export attachments/{fileName}";

            return Ok(ResponseHandler.Success(fileUrl));
        }

        var result = await _exportService.ExportToExcel(data);

        return Ok(ResponseHandler.Success(result));
    }

    [HttpPost("change-image/{id}")]
    public async Task<IActionResult> UploadImage(int id,IFormFile image)
    {
        var modelItem = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        if (modelItem is null)
        {
            return NotFound(ResponseHandler.NotFound<string>("not found user.."));
        }

        if (image is null || image.Length == 0)
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Invalid image."));
        }
        
        if (image.Length > 5 * 1024 * 1024) 
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Image size exceeds the limit (5 MB)."));
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(image.FileName).ToLower();
        if (!allowedExtensions.Contains(fileExtension))
        {
            return BadRequest(ResponseHandler.BadRequest<string>("Invalid file format. Allowed formats: jpg, jpeg, png, gif."));
        }

        modelItem.ProfilePicturePath = await Picture.Upload(
            image,
            _webHostEnvironment,
            $"uploads/users/{modelItem.UserName}-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");

        var updatedModel = await _userManager.UpdateAsync(modelItem);

        if (!updatedModel.Succeeded)
        {
            return BadRequest(ResponseHandler.BadRequest<string>(updatedModel.Errors.ToString()));
        }

        if (!string.IsNullOrEmpty(modelItem.ProfilePicturePath))
        {
            modelItem.ProfilePicturePath = Path.Combine(_imagesBaseURL, modelItem.ProfilePicturePath);
        }

        var result = _mapper.Map<GetUserDto>(modelItem);
        return Ok(ResponseHandler.Success(result));
    }
}
