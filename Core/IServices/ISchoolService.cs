using VModels.DTOS.Report;

namespace Core.IServices;

public interface ISchoolService
{
    Response<List<GetSchoolDto>> GetAll(int pageNumber, int pageSize);
    Task<Response<GetSchoolReportDto>> GetSchoolReport(int schoolId, int SeasonId);
    Response<List<GetSchoolDto>> GetByOrganization(int orgId, int pageNumber, int pageSize);
    Task<Response<GetSchoolDto?>> GetById(int id);
    Task<Response<GetSchoolDto>> Add(AddSchoolDto model);
    Task<Response<bool>> Update(UpdateSchoolDto model);
    Task<Response<bool>> Delete(int id);
}