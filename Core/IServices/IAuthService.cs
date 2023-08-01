using VModels.DTOS.Auth;

namespace Core.IServices;

public interface IAuthService
{
    Task<Response<string>> RegisterAsync(RegisterDto dto);
    Task<Response<JwtAuthResult>> LoginAsync(LoginDto dto);
    Task<Response<JwtAuthResult>> RefreshTokenAsync(RefreshTokenInputDto dto);
    Task<Response<bool>> RevokeTokenAsync(string username);
}
