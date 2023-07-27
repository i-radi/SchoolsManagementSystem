using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SMS.Presentation.Controllers;

[Route("api/[controller]")]
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

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> GetTokenAsync(LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(model);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAll()
    {
        var modelItems = await _userManager.Users.ToListAsync();
        var result = _mapper.Map<IEnumerable<GetUserDto>>(modelItems);
        foreach (var userDto in result.Select((value, i) => new { i, value }))
        {
            userDto.value.Role = (await _userManager.GetRolesAsync(modelItems[userDto.i])).FirstOrDefault()!;
        }
        return Ok(result);
    }

    [HttpGet("id")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<GetUserDto>> GetById(int id)
    {
        var modelItem = await _userManager.FindByIdAsync(id.ToString());
        var result = _mapper.Map<GetUserDto>(modelItem);
        result.Role = (await _userManager.GetRolesAsync(modelItem!)).FirstOrDefault()!;
        return Ok(result);
    }
}
