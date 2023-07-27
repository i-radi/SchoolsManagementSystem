namespace SMS.Core.Services;

public class GradeService : IGradeService
{
    private readonly IGradeRepo _gradeRepo;
    private readonly IMapper _mapper;

    public GradeService(IGradeRepo gradeRepo, IMapper mapper)
    {
        _gradeRepo = gradeRepo;
        _mapper = mapper;
    }

    public List<GetGradeDto> GetAll()
    {
        var modelItems = _gradeRepo.GetTableNoTracking();

        return _mapper.Map<List<GetGradeDto>>(modelItems);
    }

    public async Task<GetGradeDto?> GetById(int id)
    {
        var modelItem = await _gradeRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return _mapper.Map<GetGradeDto>(modelItem);
    }

    public async Task<GetGradeDto> Add(AddGradeDto dto)
    {
        var modelItem = _mapper.Map<Grade>(dto);

        var model = await _gradeRepo.AddAsync(modelItem);
        await _gradeRepo.SaveChangesAsync();

        return _mapper.Map<GetGradeDto>(modelItem);
    }

    public async Task<bool> Update(UpdateGradeDto dto)
    {
        var modelItem = await _gradeRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return false;

        _mapper.Map(dto, modelItem);

        var model = _gradeRepo.UpdateAsync(modelItem);
        await _gradeRepo.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(int id)
    {

        var dbModel = await _gradeRepo.GetByIdAsync(id);

        if (dbModel == null)
            return false;

        await _gradeRepo.DeleteAsync(dbModel);
        await _gradeRepo.SaveChangesAsync();
        return true;
    }
}
