namespace Core.IServices;

public interface IGradeService
{
    Result<List<GetGradeDto>> GetAll(int pageNumber, int pageSize);
    Task<Result<GetGradeDto?>> GetById(int id);
    Task<Result<GetGradeDto>> Add(AddGradeDto model);
    Task<Result<bool>> Update(UpdateGradeDto model);
    Task<Result<bool>> Delete(int id);
}