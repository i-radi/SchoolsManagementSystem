namespace Core.IServices;

public interface IActivityService
{
    Response<List<GetActivityDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0);
    Task<Response<GetActivityDto?>> GetById(int id);
    Task<Response<GetActivityDto>> Add(AddActivityDto model);
    Task<Response<bool>> Update(UpdateActivityDto model);
    Task<Response<bool>> Delete(int id);
}
