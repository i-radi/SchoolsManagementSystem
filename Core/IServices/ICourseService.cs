namespace Core.IServices;

public interface ICourseService
{
    Result<PaginatedList<GetCourseDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0);
    Task<Result<GetCourseDto?>> GetById(int id);
    Task<Result<GetCourseDto>> Add(AddCourseDto model);
    Task<Result<bool>> Update(UpdateCourseDto model);
    Task<Result<bool>> Delete(int id);
}