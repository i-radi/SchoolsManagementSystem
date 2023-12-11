namespace Core.Services;

public class ActivityService : IActivityService
{
    private readonly IActivityRepo _activitiesRepo;
    private readonly IMapper _mapper;

    public ActivityService(IActivityRepo activitysRepo, IMapper mapper)
    {
        _activitiesRepo = activitysRepo;
        _mapper = mapper;
    }

    public Result<List<GetActivityDto>> GetAll(int pageNumber, int pageSize, int schoolId = 0)
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

        return ResultHandler.Success(_mapper.Map<List<GetActivityDto>>(result));
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
}

