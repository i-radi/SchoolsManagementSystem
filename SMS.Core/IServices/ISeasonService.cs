namespace SMS.Core.IServices;

public interface ISeasonService
{
    Response<List<GetSeasonDto>> GetAll(int pageNumber, int pageSize);
    Task<Response<GetSeasonDto?>> GetById(int id);
    Task<Response<GetSeasonDto>> Add(AddSeasonDto model);
    Task<Response<bool>> Update(UpdateSeasonDto model);
    Task<Response<bool>> Delete(int id);
}