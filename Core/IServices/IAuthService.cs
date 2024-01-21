using VModels.DTOS.Auth;
using VModels.DTOS.Users.Commands;

namespace Core.IServices;
public interface IAuthService
{
    Task<Result<JwtAuthResult>> LoginAsync(LoginDto dto);
    Task<Result<JwtAuthResult>> LoginByUserNameAsync(LoginDto dto);
    Task<Result<string>> AddAsync(AddUserDto dto);
    Task<Result<string>> AddUserToOrganizationAsync(AddUserToOrganizationsDto dto);
    Task<Result<GetUserDto>> ChangeUserPasswordAsync(ChangeUserPasswordDto dto);
    Task<Result<bool>> ChangeUserPasswordByIdAsync(ChangeUserPasswordByIdDto dto);

    Task<Result<JwtAuthResult>> UpdateAsync(ChangeUserDto dto);
    Task<Result<JwtAuthResult>> RefreshTokenAsync(RefreshTokenInputDto dto);
    Task<Result<bool>> RevokeTokenAsync(string username);
    Task<Result<List<RoleResult>>> GetUserRoles(int userId);
    Task<Result<List<ClassroomResult>>> GetUserClassrooms(int userId);

}
