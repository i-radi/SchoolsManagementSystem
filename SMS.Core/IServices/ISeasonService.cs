namespace SMS.Core.IServices;

public interface ISeasonService
{
    List<GetSeasonDto> GetAll();
    Task<GetSeasonDto?> GetById(int id);
    Task<GetSeasonDto> Add(AddSeasonDto model);
    Task<bool> Update(UpdateSeasonDto model);
    Task<bool> Delete(int id);
}