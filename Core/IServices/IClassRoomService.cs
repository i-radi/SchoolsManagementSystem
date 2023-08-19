namespace Core.IServices;

public interface IClassroomService
{
    Response<List<GetClassroomDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0);
    Task<Response<GetClassroomDto?>> GetById(int id);
    Task<Response<GetClassroomDto>> Add(AddClassroomDto model);
    Task<Response<bool>> Update(UpdateClassroomDto model);
    Task<Response<bool>> Delete(int id);
}