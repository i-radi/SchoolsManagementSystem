namespace Core.IServices;

public interface IClassRoomService
{
    Response<List<GetClassRoomDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0);
    Task<Response<GetClassRoomDto?>> GetById(int id);
    Task<Response<GetClassRoomDto>> Add(AddClassRoomDto model);
    Task<Response<bool>> Update(UpdateClassRoomDto model);
    Task<Response<bool>> Delete(int id);
}