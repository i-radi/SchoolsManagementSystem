using Microsoft.AspNetCore.Hosting;
using Models.Entities.Identity;

namespace Presentation.Controllers.API;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(
        IAuthService authService)
    {
        _authService = authService;
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

    [HttpPost("add-user")]
    public async Task<IActionResult> AddAsync(AddUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.AddAsync(dto);

        return Ok(result);
    }

    [HttpPut("change-user/{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute]int id, [FromBody]ChangeUserDto dto)
    {
        if (id != dto.UserId)
            return BadRequest("Invalid User Id.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.UpdateAsync(dto);

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
    //[Authorize(Policy = "SuperAdmin")]
    public async Task<IActionResult> RevokeTokenAsync(string username)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RevokeTokenAsync(username);

        if (!result.Succeeded)
            return BadRequest("Opps, something went wrong.");

        return Ok("done.");
    }
}
