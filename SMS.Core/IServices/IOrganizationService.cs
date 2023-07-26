using SMS.VModels.DTOS.Organizations.Commands;
using SMS.VModels.DTOS.Organizations.Queries;

namespace SMS.Core.IServices;

public interface IOrganizationService
{
    List<GetClassDto> GetAll();
    Task<GetClassDto?> GetById(int id);
    Task<GetClassDto> Add(AddClassDto model);
    Task<bool> Update(UpdateClassDto model);
    Task<bool> Delete(int id);
}
