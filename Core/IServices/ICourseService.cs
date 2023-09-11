namespace Core.IServices;

public interface ICourseService
{
    Response<List<GetCourseDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0);
    Task<Response<GetCourseDto?>> GetById(int id);
    Task<Response<GetCourseDto>> Add(AddCourseDto model);
    Task<Response<bool>> Update(UpdateCourseDto model);
    Task<Response<bool>> Delete(int id);
}