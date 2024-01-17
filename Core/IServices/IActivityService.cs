namespace Core.IServices;

public interface IActivityService
{
    Result<PaginatedList<GetActivityDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0);
    Task<Result<GetActivityDto?>?> GetById(int id);
    Task<Result<GetActivityDto>> Add(AddActivityDto model);
    Task<GetActivityInstanceDto> AddActivityInstanceToActivity(AddActivityInstanceDto dto);
    Task<Result<bool>> Update(UpdateActivityDto model);
    Task<Result<bool>> Delete(int id);
    Task<Result<bool>> Archive(int activityId);
}
