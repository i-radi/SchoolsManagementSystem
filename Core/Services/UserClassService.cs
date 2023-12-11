namespace Core.Services;

public class UserClassService : IUserClassService
{
    private readonly IUserClassRepo _userClassesRepo;
    private readonly IMapper _mapper;

    public UserClassService(IUserClassRepo userClassesRepo, IMapper mapper)
    {
        _userClassesRepo = userClassesRepo;
        _mapper = mapper;
    }

    public Result<List<GetUserClassDto>> GetAll(int pageNumber, int pageSize, int userId = 0)
    {
        var modelItems = _userClassesRepo.GetTableNoTracking()
            .Include(m => m.Classroom)
            .Include(m => m.User)
            .Include(m => m.Season)
            .Include(m => m.UserType)
            .AsQueryable();

        if (userId > 0)
        {
            modelItems = modelItems.Where(m => m.UserId == userId);
        }

        var result = PaginatedList<GetUserClassDto>.Create(_mapper.Map<List<GetUserClassDto>>(modelItems.ToList()), pageNumber, pageSize);

        return ResultHandler.Success(_mapper.Map<List<GetUserClassDto>>(result));
    }

    public async Task<Result<GetUserClassDto?>> GetById(int id)
    {
        var modelItem = await _userClassesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetUserClassDto>(modelItem))!;
    }

    public async Task<Result<GetUserClassDto>> Add(AddUserClassDto dto)
    {
        var modelItem = _mapper.Map<UserClass>(dto);

        var model = await _userClassesRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetUserClassDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateUserClassDto dto)
    {
        var modelItem = await _userClassesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _userClassesRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _userClassesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _userClassesRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}
