namespace Core.IServices;

public interface IOrganizationService
{
    Response<List<GetOrganizationDto>> GetAll(int pageNumber, int pageSize);
    Task<Response<GetOrganizationDto?>> GetById(int id);
    Task<Response<GetOrganizationDto>> Add(AddOrganizationDto model);
    Task<Response<bool>> Update(UpdateOrganizationDto model);
    Task<Response<bool>> Delete(int id);
}
