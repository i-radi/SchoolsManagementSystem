namespace Core.IServices;

public interface IRecordService
{
    Result<List<GetRecordDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0);
    Task<Result<GetRecordDto?>> GetById(int id);
    Task<Result<GetRecordDto>> Add(AddRecordDto model);
    Task<Result<bool>> Update(UpdateRecordDto model);
    Task<Result<bool>> Delete(int id);
}