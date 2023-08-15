using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers.API;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly string _imagesBaseURL;

    public UsersController(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IAuthService authService,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _mapper = mapper;
        _imagesBaseURL = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(dto);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> GetTokenAsync(LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(model);

        if (!result.Succeeded)
            return BadRequest(result);

        return Ok(result);
    }

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

    [HttpDelete("revoke-token")]
    [Authorize(Policy = "SuperAdmin")]

    public async Task<IActionResult> RevokeTokenAsync(string username)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RevokeTokenAsync(username);

        if (!result.Succeeded)
            return BadRequest("Opps, something went wrong.");

        return Ok("done.");
    }

    [HttpGet]
    [Authorize(Policy = "SuperAdmin")]
    public async Task<IActionResult> GetAll([FromHeader] int schoolId, int pageNumber = 1, int pageSize = 10)
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

    [HttpGet("{id}")]
    [Authorize(Policy = "SuperAdmin")]
    public async Task<IActionResult> GetById([FromHeader] int schoolId, int id)
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
}
