namespace Core.Services;

public class GradeService : IGradeService
{
    private readonly IGradeRepo _gradesRepo;
    private readonly IMapper _mapper;

    public GradeService(IGradeRepo gradesRepo, IMapper mapper)
    {
        _gradesRepo = gradesRepo;
        _mapper = mapper;
    }

    public Result<List<GetGradeDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _gradesRepo.GetTableNoTracking().Include(m => m.School);
        var result = PaginatedList<GetGradeDto>.Create(_mapper.Map<List<GetGradeDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(_mapper.Map<List<GetGradeDto>>(result));
    }

    public async Task<Result<GetGradeDto?>> GetById(int id)
    {
        var modelItem = await _gradesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetGradeDto>(modelItem))!;
    }

    public async Task<Result<GetGradeDto>> Add(AddGradeDto dto)
    {
        var modelItem = _mapper.Map<Grade>(dto);

        var model = await _gradesRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetGradeDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateGradeDto dto)
    {
        var modelItem = await _gradesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _gradesRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _gradesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _gradesRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}
