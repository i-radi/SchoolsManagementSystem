namespace Core.IServices;

public interface IActivityInstanceUserService
{
    Response<List<GetActivityInstanceUserDto>> GetAll();
    Task<Response<GetActivityInstanceUserDto?>> GetById(int id);
    Task<Response<GetActivityInstanceUserDto>> Add(AddActivityInstanceUserDto model);
    Task<Response<bool>> Update(UpdateActivityInstanceUserDto model);
    Task<Response<bool>> Delete(int id);
}
