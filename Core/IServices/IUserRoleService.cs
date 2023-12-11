namespace Core.IServices;

public interface IUserRoleService
{
    Result<List<GetUserRoleDto>> GetAll(int pageNumber, int pageSize);
    Task<Result<GetUserRoleDto?>> GetById(int id);
    Task<Result<GetUserRoleDto>> Add(AddUserRoleDto model);
    Task<Result<bool>> Update(UpdateUserRoleDto model);
    Task<Result<bool>> Delete(AddUserRoleDto model);
    Task<Result<bool>> IsExists(AddUserRoleDto model);
}