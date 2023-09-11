namespace Core.IServices;

public interface IRecordService
{
    Response<List<GetRecordDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0);
    Task<Response<GetRecordDto?>> GetById(int id);
    Task<Response<GetRecordDto>> Add(AddRecordDto model);
    Task<Response<bool>> Update(UpdateRecordDto model);
    Task<Response<bool>> Delete(int id);
}