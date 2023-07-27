namespace SMS.Core.IServices;

public interface IUserClassService
{
    List<GetUserClassDto> GetAll();
    Task<GetUserClassDto?> GetById(int id);
    Task<GetUserClassDto> Add(AddUserClassDto model);
    Task<bool> Update(UpdateUserClassDto model);
    Task<bool> Delete(int id);
}