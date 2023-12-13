namespace Core.Services;

public class ActivityTimeService : IActivityTimeService
{
    private readonly IActivityTimeRepo _activityTimesRepo;
    private readonly IMapper _mapper;

    public ActivityTimeService(IActivityTimeRepo activitysRepo, IMapper mapper)
    {
        _activityTimesRepo = activitysRepo;
        _mapper = mapper;
    }

    public Result<PaginatedList<GetActivityTimeDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _activityTimesRepo.GetTableNoTracking();


        var result = PaginatedList<GetActivityTimeDto>.Create(_mapper.Map<List<GetActivityTimeDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(result);
    }

    public async Task<Result<GetActivityTimeDto?>> GetById(int id)
    {
        var modelItem = await _activityTimesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetActivityTimeDto>(modelItem))!;
    }

    public async Task<Result<GetActivityTimeDto>> Add(AddActivityTimeDto dto)
    {
        var modelItem = _mapper.Map<ActivityTime>(dto);

        var model = await _activityTimesRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetActivityTimeDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateActivityTimeDto dto)
    {
        var modelItem = await _activityTimesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _activityTimesRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _activityTimesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _activityTimesRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}

