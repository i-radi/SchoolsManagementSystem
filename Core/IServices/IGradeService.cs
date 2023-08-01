namespace Core.IServices;

public interface IGradeService
{
    Response<List<GetGradeDto>> GetAll(int pageNumber, int pageSize);
    Task<Response<GetGradeDto?>> GetById(int id);
    Task<Response<GetGradeDto>> Add(AddGradeDto model);
    Task<Response<bool>> Update(UpdateGradeDto model);
    Task<Response<bool>> Delete(int id);
}