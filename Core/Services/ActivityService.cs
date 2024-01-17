namespace Core.Services;

public class ActivityService : IActivityService
{
    private readonly IActivityRepo _activitiesRepo;
    private readonly IMapper _mapper;
    private readonly IActivityInstanceRepo _activityInstanceRepo;

    public ActivityService(IActivityRepo activitysRepo, IMapper mapper , IActivityInstanceRepo activityInstanceRepo)
    {
        _activitiesRepo = activitysRepo;
        _mapper = mapper;
        _activityInstanceRepo = activityInstanceRepo;
    }

    public Result<PaginatedList<GetActivityDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0)
    {
        var modelItems = _activitiesRepo.GetTableNoTracking();

        if (schoolId > 0)
        {
            modelItems = modelItems.Include(c => c.School)
                .Where(cr => cr.SchoolId == schoolId);
        }
        else
        {
            modelItems = modelItems.Include(c => c.School);
        }

        var result = PaginatedList<GetActivityDto>.Create(_mapper.Map<List<GetActivityDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(result);
    }

    public async Task<Result<GetActivityDto?>?> GetById(int id)
    {
        var modelItem = await _activitiesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetActivityDto>(modelItem))!;
    }

    public async Task<Result<GetActivityDto>> Add(AddActivityDto dto)
    {
        var modelItem = _mapper.Map<Activity>(dto);
        _ = await _activitiesRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetActivityDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateActivityDto dto)
    {
        var modelItem = await _activitiesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);
        _ = _activitiesRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _activitiesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _activitiesRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }

    public async Task<Result<bool>> Archive(int activityId)
    {
        var modelItem = await _activitiesRepo.GetByIdAsync(activityId);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>("Not Found Activity");

        modelItem.IsAvailable = false;

        _ = _activitiesRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<GetActivityInstanceDto> AddActivityInstanceToActivity(AddActivityInstanceDto dto)
    {
        var avtivity = await _activitiesRepo.GetByIdAsync(dto.ActivityId);
        if (avtivity is null)
            return null;
        var modelItem = new ActivityInstance()
        {
            Name = dto.Name,
            CreatedDate = dto.CreatedDate,
            ForDate = dto.ForDate,
            IsLocked = dto.IsLocked,
            SeasonId = dto.SeasonId,
            ActivityId = dto.ActivityId,
        };
        var activityinst =  await _activityInstanceRepo.AddAsync(modelItem);
        var returnedInstance = new GetActivityInstanceDto()
        {
            Id = activityinst.Id,
            Name = activityinst.Name,
            CreatedDate = activityinst.CreatedDate,
            ForDate = activityinst.ForDate,
            IsLocked = activityinst.IsLocked,
            SeasonId = activityinst.SeasonId,
            ActivityId = activityinst.ActivityId
        };

        return returnedInstance;   
    }
}

