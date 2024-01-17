namespace Core.IServices;

public interface IOrganizationService
{
    Task<Result<List<GetOrganizationDto>>> GetAll();
    Task<Result<GetOrganizationDto?>> GetById(int id);
 
    Task<Result<GetOrganizationDto>> Add(AddOrganizationDto model);
    Task<Result<bool>> Update(UpdateOrganizationDto model);
    Task<Result<bool>> Delete(int id);
}
