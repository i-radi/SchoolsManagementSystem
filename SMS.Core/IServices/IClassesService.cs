namespace SMS.Core.IServices;

public interface IClassesService
{
    List<GetClassDto> GetAll();
    Task<GetClassDto?> GetById(int id);
    Task<GetClassDto> Add(AddClassDto model);
    Task<bool> Update(UpdateClassDto model);
    Task<bool> Delete(int id);
}