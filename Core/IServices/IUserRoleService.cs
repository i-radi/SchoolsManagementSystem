namespace Core.IServices;

public interface IUserRoleService
{
    Response<List<GetUserRoleDto>> GetAll(int pageNumber, int pageSize);
    Task<Response<GetUserRoleDto?>> GetById(int id);
    Task<Response<GetUserRoleDto>> Add(AddUserRoleDto model);
    Task<Response<bool>> Update(UpdateUserRoleDto model);
    Task<Response<bool>> Delete(AddUserRoleDto model);
    Task<Response<bool>> IsExists(AddUserRoleDto model);
}