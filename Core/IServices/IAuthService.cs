using Models.Entities.Identity;
using VModels.DTOS.Auth;

namespace Core.IServices;

public interface IAuthService
{
    Task<Response<JwtAuthResult>> LoginAsync(LoginDto dto);
    Task<Response<JwtAuthResult>> LoginByUserNameAsync(LoginDto dto);
    Task<Response<string>> AddAsync(AddUserDto dto);
    Task<Response<GetUserDto>> ChangeUserPasswordAsync(ChangeUserPasswordDto dto);
    Task<Response<JwtAuthResult>> UpdateAsync(ChangeUserDto dto);
    Task<Response<JwtAuthResult>> RefreshTokenAsync(RefreshTokenInputDto dto);
    Task<Response<bool>> RevokeTokenAsync(string username);
    Task<Response<List<RoleResult>>> GetUserRoles(int userId);
    Task<Response<List<ClassroomResult>>> GetUserClassrooms(int userId);

}
