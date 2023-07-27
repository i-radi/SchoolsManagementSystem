namespace SMS.Core.IServices;

public interface IGradeService
{
    List<GetGradeDto> GetAll();
    Task<GetGradeDto?> GetById(int id);
    Task<GetGradeDto> Add(AddGradeDto model);
    Task<bool> Update(UpdateGradeDto model);
    Task<bool> Delete(int id);
}