namespace Core.IServices;

public interface IRecordClassService
{
    Response<List<GetRecordClassDto>> GetAll(int pageNumber, int pageSize);
    Task<Response<GetRecordClassDto?>> GetById(int id);
    Task<Response<GetRecordClassDto>> Add(AddRecordClassDto model);
    Task<Response<bool>> Update(UpdateRecordClassDto model);
    Task<Response<bool>> Delete(int id);
}