namespace Core.Services;

public class RecordClassService : IRecordClassService
{
    private readonly IRecordClassRepo _recordClassesRepo;
    private readonly IMapper _mapper;

    public RecordClassService(IRecordClassRepo recordClassesRepo, IMapper mapper)
    {
        _recordClassesRepo = recordClassesRepo;
        _mapper = mapper;
    }

    public Result<PaginatedList<GetRecordClassDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _recordClassesRepo.GetTableNoTracking()
            .Include(c => c.Record)
            .Include(c => c.Classroom);

        var result = PaginatedList<GetRecordClassDto>.Create(_mapper.Map<List<GetRecordClassDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(result);
    }

    public async Task<Result<GetRecordClassDto?>> GetById(int id)
    {
        var modelItem = await _recordClassesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetRecordClassDto>(modelItem))!;
    }

    public async Task<Result<GetRecordClassDto>> Add(AddRecordClassDto dto)
    {
        var modelItem = _mapper.Map<RecordClass>(dto);

        var model = await _recordClassesRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetRecordClassDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateRecordClassDto dto)
    {
        var modelItem = await _recordClassesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _recordClassesRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _recordClassesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _recordClassesRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}
