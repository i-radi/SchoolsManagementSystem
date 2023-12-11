namespace Core.Services;

public class ActivityInstanceService : IActivityInstanceService
{
    private readonly IActivityInstanceRepo _activityInstancesRepo;
    private readonly IMapper _mapper;

    public ActivityInstanceService(IActivityInstanceRepo activitysRepo, IMapper mapper)
    {
        _activityInstancesRepo = activitysRepo;
        _mapper = mapper;
    }

    public Result<List<GetActivityInstanceDto>> GetAll(int pageNumber, int pageSize, int activityId = 0)
    {
        var modelItems = _activityInstancesRepo.GetTableNoTracking();

        if (activityId > 0)
        {
            modelItems = modelItems.Where(a => a.ActivityId == activityId);
        }

        var result = PaginatedList<GetActivityInstanceDto>.Create(_mapper.Map<List<GetActivityInstanceDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(_mapper.Map<List<GetActivityInstanceDto>>(result));
    }

    public async Task<Result<GetActivityInstanceDto?>> GetById(int id)
    {
        var modelItem = await _activityInstancesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetActivityInstanceDto>(modelItem))!;
    }

    public async Task<Result<GetActivityInstanceDto>> Add(AddActivityInstanceDto dto)
    {
        var modelItem = _mapper.Map<ActivityInstance>(dto);

        _ = await _activityInstancesRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetActivityInstanceDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateActivityInstanceDto dto)
    {
        var modelItem = await _activityInstancesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);
        _ = _activityInstancesRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _activityInstancesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _activityInstancesRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}

