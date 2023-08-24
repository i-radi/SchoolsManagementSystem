namespace Core.Services;

public class ActivityInstanceUserService : IActivityInstanceUserService
{
    private readonly IActivityInstanceUserRepo _activityInstanceUsersRepo;
    private readonly IMapper _mapper;

    public ActivityInstanceUserService(IActivityInstanceUserRepo activitysRepo, IMapper mapper)
    {
        _activityInstanceUsersRepo = activitysRepo;
        _mapper = mapper;
    }

    public Response<List<GetActivityInstanceUserDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _activityInstanceUsersRepo.GetTableNoTracking();


        var result = PaginatedList<GetActivityInstanceUserDto>.Create(_mapper.Map<List<GetActivityInstanceUserDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetActivityInstanceUserDto>>(result));
    }

    public async Task<Response<GetActivityInstanceUserDto?>> GetById(int id)
    {
        var modelItem = await _activityInstanceUsersRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetActivityInstanceUserDto>(modelItem))!;
    }

    public async Task<Response<GetActivityInstanceUserDto>> Add(AddActivityInstanceUserDto dto)
    {
        var modelItem = _mapper.Map<ActivityInstanceUser>(dto);

        var model = await _activityInstanceUsersRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetActivityInstanceUserDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateActivityInstanceUserDto dto)
    {
        var modelItem = await _activityInstanceUsersRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _activityInstanceUsersRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _activityInstanceUsersRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _activityInstanceUsersRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}

