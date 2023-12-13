namespace Core.IServices;

public interface IUserRecordService
{
    Result<PaginatedList<GetUserRecordDto>> GetAll(int pageNumber, int pageSize);
    Task<Result<GetUserRecordDto?>> GetById(int id);
    Task<Result<GetUserRecordDto>> Add(AddUserRecordDto model);
    Task<Result<bool>> Update(UpdateUserRecordDto model);
    Task<Result<bool>> Delete(int id);
}