using SMS.Models.Entities.Identity;
using SMS.Models.Results;
using SMS.VModels.DTOS.Auth;
using System.IdentityModel.Tokens.Jwt;

namespace SMS.Core.IServices;

public interface IAuthService
{
    Task<JwtAuthResult> RegisterAsync(RegisterDto dto);
    Task<JwtAuthResult> LoginAsync(LoginDto dto);
    public Task<JwtAuthResult> GetJWTToken(User user);
    public JwtSecurityToken ReadJWTToken(string accessToken);
    public Task<string> ValidateToken(string AccessToken);
}
