using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SMS.Presentation.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public UsersController(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        IAuthService authService,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    [Authorize(Policy = "Admin")]
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
        throw new UnauthorizedAccessException();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(model);

        if (!result.Data.IsAuthenticated)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> GetRefreshTokenAsync(RefreshTokenInputDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RefreshTokenAsync(model);

        if (!result.Data.IsAuthenticated)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("revoke-token")]
    [Authorize(Policy = "Admin")]

    public async Task<IActionResult> RevokeTokenAsync(string username)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RevokeTokenAsync(username);

        if (!result.Data)
            return BadRequest("Opps, something went wrong.");

        return Ok("done.");
    }

    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var modelItems = PaginatedList<User>
            .Create(await _userManager.Users
            .Include(u => u.Organization)
            .ToListAsync(), pageNumber, pageSize);

        var result = _mapper.Map<IEnumerable<GetUserDto>>(modelItems);
        foreach (var userDto in result.Select((value, i) => new { i, value }))
        {
            userDto.value.Role = (await _userManager.GetRolesAsync(modelItems[userDto.i])).FirstOrDefault()!;
        }
        return Ok(ResponseHandler.Success(result));
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var modelItem = await _userManager.Users
            .Include(u => u.Organization)
            .FirstOrDefaultAsync(u => u.Id == id);

        var result = _mapper.Map<GetUserDto>(modelItem);
        result.Role = (await _userManager.GetRolesAsync(modelItem!)).FirstOrDefault()!;
        return Ok(ResponseHandler.Success(result));
    }
}
