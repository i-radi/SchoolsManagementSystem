namespace Core.IServices;

public interface IUserTypeService
{
    Response<List<GetUserTypeDto>> GetAll(int pageNumber, int pageSize);
    Task<Response<GetUserTypeDto?>> GetById(int id);
    Task<Response<GetUserTypeDto>> Add(AddUserTypeDto model);
    Task<Response<bool>> Update(UpdateUserTypeDto model);
    Task<Response<bool>> Delete(int id);
}