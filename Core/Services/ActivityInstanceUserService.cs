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

    public Result<List<GetActivityInstanceUserDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _activityInstanceUsersRepo.GetTableNoTracking();


        var result = PaginatedList<GetActivityInstanceUserDto>.Create(_mapper.Map<List<GetActivityInstanceUserDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(_mapper.Map<List<GetActivityInstanceUserDto>>(result));
    }

    public async Task<Result<GetActivityInstanceUserDto?>> GetById(int id)
    {
        var modelItem = await _activityInstanceUsersRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetActivityInstanceUserDto>(modelItem))!;
    }

    public async Task<Result<GetActivityInstanceUserDto>> Add(AddActivityInstanceUserDto dto)
    {
        var modelItem = _mapper.Map<ActivityInstanceUser>(dto);

        var model = await _activityInstanceUsersRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetActivityInstanceUserDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateActivityInstanceUserDto dto)
    {
        var modelItem = await _activityInstanceUsersRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _activityInstanceUsersRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _activityInstanceUsersRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _activityInstanceUsersRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}

