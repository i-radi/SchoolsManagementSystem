using VModels.DTOS.Report;

namespace Core.IServices;

public interface ISchoolService
{
    Result<PaginatedList<GetSchoolDto>> GetAll(int pageNumber, int pageSize);
    Task<Result<GetSchoolReportDto>> GetSchoolReport(int schoolId, int SeasonId);
    Result<List<GetSchoolDto>> GetByOrganization(int orgId, int pageNumber, int pageSize);
    Task<Result<GetSchoolDto?>> GetById(int id);
    Task<Result<GetSchoolDto>> Add(AddSchoolDto model);
    Task<Result<bool>> Update(UpdateSchoolDto model);
    Task<Result<bool>> Delete(int id);
}