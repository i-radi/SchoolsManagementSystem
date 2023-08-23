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

    public Response<List<GetActivityClassroomDto>> GetAll()
    {
        var modelItems = _activityClassroomsRepo.GetTableNoTracking();


        var result = PaginatedList<GetActivityClassroomDto>.Create(_mapper.Map<List<GetActivityClassroomDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetActivityClassroomDto>>(result));
    }

    public async Task<Response<GetActivityClassroomDto?>> GetById(int id)
    {
        var modelItem = await _activityClassroomsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetActivityClassroomDto>(modelItem))!;
    }

    public async Task<Response<GetActivityClassroomDto>> Add(AddActivityClassroomDto dto)
    {
        var modelItem = _mapper.Map<ActivityClassroom>(dto);

        var model = await _activityClassroomsRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetActivityClassroomDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateActivityClassroomDto dto)
    {
        var modelItem = await _activityClassroomsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _activityClassroomsRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _activityClassroomsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _activityClassroomsRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}

