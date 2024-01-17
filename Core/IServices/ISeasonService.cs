namespace Core.IServices;

public interface ISeasonService
{
    Result<PaginatedList<GetSeasonDto>> GetAll(int pageNumber, int pageSize);
    Task<List<GetSeasonDto?>> GetAllSeasonsBySchoolId(int schoolid);
    Task<Result<GetSeasonDto?>> GetById(int id);
    Task<Result<GetSeasonDto>> Add(AddSeasonDto model);
    Task<Result<bool>> Update(UpdateSeasonDto model);
    Task<Result<bool>> Delete(int id);
}