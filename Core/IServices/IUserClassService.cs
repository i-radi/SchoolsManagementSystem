namespace Core.IServices;

public interface IUserClassService
{
    Result<PaginatedList<GetUserClassDto>> GetAll(int pageNumber, int pageSize, int userId = 0);
    Task<Result<GetUserClassDto?>> GetById(int id);
    Task<Result<GetUserClassDto>> Add(AddUserClassDto model);
    Task<Result<bool>> Update(UpdateUserClassDto model);
    Task<Result<bool>> Delete(AddUserClassDto dto);
}