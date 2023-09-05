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

    public Response<List<GetActivityInstanceDto>> GetAll(int pageNumber, int pageSize, int activityId = 0)
    {
        var modelItems = _activityInstancesRepo.GetTableNoTracking();

        if (activityId > 0 ) 
        {
            modelItems = modelItems.Where(a => a.ActivityId == activityId);
        }

        var result = PaginatedList<GetActivityInstanceDto>.Create(_mapper.Map<List<GetActivityInstanceDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetActivityInstanceDto>>(result));
    }

    public async Task<Response<GetActivityInstanceDto?>> GetById(int id)
    {
        var modelItem = await _activityInstancesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetActivityInstanceDto>(modelItem))!;
    }

    public async Task<Response<GetActivityInstanceDto>> Add(AddActivityInstanceDto dto)
    {
        var modelItem = _mapper.Map<ActivityInstance>(dto);

        var model = await _activityInstancesRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetActivityInstanceDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateActivityInstanceDto dto)
    {
        var modelItem = await _activityInstancesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _activityInstancesRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _activityInstancesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _activityInstancesRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}

