namespace Core.IServices;

public interface IUserRoleService
{
    Result<PaginatedList<GetUserRoleDto>> GetAll(int pageNumber, int pageSize);
    Task<Result<GetUserRoleDto?>> GetById(int id);
    Task<Result<GetUserRoleDto>> Add(AddUserRoleDto model);
    Task<Result<bool>> Update(UpdateUserRoleDto model);
    Task<Result<bool>> Delete(int userRoleId);
    Task<Result<bool>> IsExists(AddUserRoleDto model);
}