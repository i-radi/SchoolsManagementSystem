namespace Core.IServices;

public interface IUserTypeService
{
    Result<List<GetUserTypeDto>> GetAll(int pageNumber, int pageSize);
    Task<Result<GetUserTypeDto?>> GetById(int id);
    Task<Result<GetUserTypeDto>> Add(AddUserTypeDto model);
    Task<Result<bool>> Update(UpdateUserTypeDto model);
    Task<Result<bool>> Delete(int id);
}