namespace Core.IServices;

public interface IActivityTimeService
{
    Result<PaginatedList<GetActivityTimeDto>> GetAll(int pageNumber, int pageSize);
    Task<Result<GetActivityTimeDto?>> GetById(int id);
    Task<Result<GetActivityTimeDto>> Add(AddActivityTimeDto model);
    Task<Result<bool>> Update(UpdateActivityTimeDto model);
    Task<Result<bool>> Delete(int id);
}
