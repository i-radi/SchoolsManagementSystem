namespace Core.IServices;

public interface IActivityInstanceService
{
    Result<List<GetActivityInstanceDto>> GetAll(int pageNumber, int pageSize, int activityId = 0);
    Task<Result<GetActivityInstanceDto?>> GetById(int id);
    Task<Result<GetActivityInstanceDto>> Add(AddActivityInstanceDto model);
    Task<Result<bool>> Update(UpdateActivityInstanceDto model);
    Task<Result<bool>> Delete(int id);
}
