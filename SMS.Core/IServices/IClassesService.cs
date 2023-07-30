namespace SMS.Core.IServices;

public interface IClassesService
{
    Response<List<GetClassDto>> GetAll(int pageNumber, int pageSize);
    Task<Response<GetClassDto?>> GetById(int id);
    Task<Response<GetClassDto>> Add(AddClassDto model);
    Task<Response<bool>> Update(UpdateClassDto model);
    Task<Response<bool>> Delete(int id);
}