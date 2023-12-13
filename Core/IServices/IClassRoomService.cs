namespace Core.IServices;

public interface IClassroomService
{
    Result<PaginatedList<GetClassroomDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0);
    Task<Result<GetClassroomDto?>> GetById(int id);
    Task<Result<GetClassroomDto>> Add(AddClassroomDto model);
    Task<Result<bool>> Update(UpdateClassroomDto model);
    Task<Result<bool>> Delete(int id);
}