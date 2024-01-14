namespace Core.IServices;

public interface IActivityInstanceUserService
{
    Result<PaginatedList<GetActivityInstanceUserDto>> GetAll(int pageNumber, int pageSize);
    Task<Result<GetActivityInstanceUserDto?>> GetById(int id);
    Task<Result<GetActivityInstanceUserDto>> Add(AddActivityInstanceUserDto model);
    Task<Result<bool>> Update(UpdateActivityInstanceUserDto model);
    Task<Result<bool>> Delete(int id);
}
