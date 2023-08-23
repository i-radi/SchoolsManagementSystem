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

    public Response<List<GetActivityTimeDto>> GetAll()
    {
        var modelItems = _activityTimesRepo.GetTableNoTracking();


        var result = PaginatedList<GetActivityTimeDto>.Create(_mapper.Map<List<GetActivityTimeDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetActivityTimeDto>>(result));
    }

    public async Task<Response<GetActivityTimeDto?>> GetById(int id)
    {
        var modelItem = await _activityTimesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetActivityTimeDto>(modelItem))!;
    }

    public async Task<Response<GetActivityTimeDto>> Add(AddActivityTimeDto dto)
    {
        var modelItem = _mapper.Map<ActivityTime>(dto);

        var model = await _activityTimesRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetActivityTimeDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateActivityTimeDto dto)
    {
        var modelItem = await _activityTimesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _activityTimesRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _activityTimesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _activityTimesRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}

