namespace Core.IServices;

public interface IActivityClassroomService
{
    Response<List<GetActivityClassroomDto>> GetAll();
    Task<Response<GetActivityClassroomDto?>> GetById(int id);
    Task<Response<GetActivityClassroomDto>> Add(AddActivityClassroomDto model);
    Task<Response<bool>> Update(UpdateActivityClassroomDto model);
    Task<Response<bool>> Delete(int id);
}
