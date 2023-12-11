namespace Core.Services;

public class ActivityClassroomService : IActivityClassroomService
{
    private readonly IActivityClassroomRepo _activityClassroomsRepo;
    private readonly IMapper _mapper;

    public ActivityClassroomService(IActivityClassroomRepo activitysRepo, IMapper mapper)
    {
        _activityClassroomsRepo = activitysRepo;
        _mapper = mapper;
    }

    public Result<List<GetActivityClassroomDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _activityClassroomsRepo.GetTableNoTracking();


        var result = PaginatedList<GetActivityClassroomDto>.Create(_mapper.Map<List<GetActivityClassroomDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(_mapper.Map<List<GetActivityClassroomDto>>(result));
    }

    public async Task<Result<GetActivityClassroomDto?>?> GetById(int id)
    {
        var modelItem = await _activityClassroomsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetActivityClassroomDto>(modelItem))!;
    }

    public async Task<Result<GetActivityClassroomDto>> Add(AddActivityClassroomDto dto)
    {
        var modelItem = _mapper.Map<ActivityClassroom>(dto);
        _ = await _activityClassroomsRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetActivityClassroomDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateActivityClassroomDto dto)
    {
        var modelItem = await _activityClassroomsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);
        _ = _activityClassroomsRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _activityClassroomsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _activityClassroomsRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}

