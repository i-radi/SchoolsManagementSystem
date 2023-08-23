namespace Core.IServices;

public interface IActivityTimeService
{
    Response<List<GetActivityTimeDto>> GetAll();
    Task<Response<GetActivityTimeDto?>> GetById(int id);
    Task<Response<GetActivityTimeDto>> Add(AddActivityTimeDto model);
    Task<Response<bool>> Update(UpdateActivityTimeDto model);
    Task<Response<bool>> Delete(int id);
}
