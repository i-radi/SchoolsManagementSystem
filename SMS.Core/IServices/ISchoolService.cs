namespace SMS.Core.IServices;

public interface ISchoolService
{
    Response<List<GetSchoolDto>> GetAll(int pageNumber, int pageSize);
    Task<Response<GetSchoolDto?>> GetById(int id);
    Task<Response<GetSchoolDto>> Add(AddSchoolDto model);
    Task<Response<bool>> Update(UpdateSchoolDto model);
    Task<Response<bool>> Delete(int id);
}