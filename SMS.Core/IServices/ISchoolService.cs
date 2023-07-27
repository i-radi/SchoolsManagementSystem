namespace SMS.Core.IServices;

public interface ISchoolService
{
    List<GetSchoolDto> GetAll();
    Task<GetSchoolDto?> GetById(int id);
    Task<GetSchoolDto> Add(AddSchoolDto model);
    Task<bool> Update(UpdateSchoolDto model);
    Task<bool> Delete(int id);
}