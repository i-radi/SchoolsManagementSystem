namespace Core.IServices;

public interface IActivityTimeService
{
    Response<List<GetActivityTimeDto>> GetAll(int pageNumber, int pageSize);
    Task<Response<GetActivityTimeDto?>> GetById(int id);
    Task<Response<GetActivityTimeDto>> Add(AddActivityTimeDto model);
    Task<Response<bool>> Update(UpdateActivityTimeDto model);
    Task<Response<bool>> Delete(int id);
}
