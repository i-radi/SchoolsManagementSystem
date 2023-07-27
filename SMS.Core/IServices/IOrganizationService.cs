namespace SMS.Core.IServices;

public interface IOrganizationService
{
    List<GetOrganizationDto> GetAll();
    Task<GetOrganizationDto?> GetById(int id);
    Task<GetOrganizationDto> Add(AddOrganizationDto model);
    Task<bool> Update(UpdateOrganizationDto model);
    Task<bool> Delete(int id);
}
