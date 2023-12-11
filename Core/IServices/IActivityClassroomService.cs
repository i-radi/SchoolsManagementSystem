namespace Core.IServices;

public interface IActivityClassroomService
{
    Result<List<GetActivityClassroomDto>> GetAll(int pageNumber, int pageSize);
    Task<Result<GetActivityClassroomDto?>?> GetById(int id);
    Task<Result<GetActivityClassroomDto>> Add(AddActivityClassroomDto model);
    Task<Result<bool>> Update(UpdateActivityClassroomDto model);
    Task<Result<bool>> Delete(int id);
}
