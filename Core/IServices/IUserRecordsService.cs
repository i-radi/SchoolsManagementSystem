namespace Core.IServices;

public interface IUserRecordService
{
    Response<List<GetUserRecordDto>> GetAll(int pageNumber, int pageSize);
    Task<Response<GetUserRecordDto?>> GetById(int id);
    Task<Response<GetUserRecordDto>> Add(AddUserRecordDto model);
    Task<Response<bool>> Update(UpdateUserRecordDto model);
    Task<Response<bool>> Delete(int id);
}