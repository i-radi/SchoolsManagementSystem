namespace SMS.Core.IServices;

public interface IUserTypeService
{
    List<GetUserTypeDto> GetAll();
    Task<GetUserTypeDto?> GetById(int id);
    Task<GetUserTypeDto> Add(AddUserTypeDto model);
    Task<bool> Update(UpdateUserTypeDto model);
    Task<bool> Delete(int id);
}