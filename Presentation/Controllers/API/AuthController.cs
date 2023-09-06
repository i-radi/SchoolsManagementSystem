using Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Models.Results;

namespace Presentation.Controllers.API;

[Authorize]
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly RoleManager<Role> _roleManager;

    public AuthController(
        IAuthService authService,
        RoleManager<Role> roleManager)
    {
        _authService = authService;
        _roleManager = roleManager;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = new Response<JwtAuthResult>();

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
    public async Task<IActionResult> RevokeTokenAsync(string username)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RevokeTokenAsync(username);

        if (!result.Succeeded)
            return BadRequest("Opps, something went wrong.");

        return Ok("done.");
    }

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
    
    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = (await _roleManager.Roles.ToListAsync())
            .Select(r => new 
            { 
                r.Id, r.Name
            });

        return Ok(roles);
    }
}
