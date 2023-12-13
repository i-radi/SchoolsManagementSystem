namespace Core.IServices;

public interface IRecordClassService
{
    Result<PaginatedList<GetRecordClassDto>> GetAll(int pageNumber, int pageSize);
    Task<Result<GetRecordClassDto?>> GetById(int id);
    Task<Result<GetRecordClassDto>> Add(AddRecordClassDto model);
    Task<Result<bool>> Update(UpdateRecordClassDto model);
    Task<Result<bool>> Delete(int id);
}