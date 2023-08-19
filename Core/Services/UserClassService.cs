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

    public Response<List<GetUserClassDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _userClassesRepo.GetTableNoTracking()
            .Include(m => m.Classroom)
            .Include(m => m.User)
            .Include(m => m.Season)
            .Include(m => m.UserType);
        var result = PaginatedList<GetUserClassDto>.Create(_mapper.Map<List<GetUserClassDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetUserClassDto>>(result));
    }

    public async Task<Response<GetUserClassDto?>> GetById(int id)
    {
        var modelItem = await _userClassesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetUserClassDto>(modelItem))!;
    }

    public async Task<Response<GetUserClassDto>> Add(AddUserClassDto dto)
    {
        var modelItem = _mapper.Map<UserClass>(dto);

        var model = await _userClassesRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetUserClassDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateUserClassDto dto)
    {
        var modelItem = await _userClassesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _userClassesRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _userClassesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _userClassesRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
