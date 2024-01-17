namespace Core.Services;

public class UserTypeService : IUserTypeService
{
    private readonly IUserTypeRepo _userTypesRepo;
    private readonly IMapper _mapper;

    public UserTypeService(IUserTypeRepo userTypesRepo, IMapper mapper)
    {
        _userTypesRepo = userTypesRepo;
        _mapper = mapper;
    }

    public Result<List<GetUserTypeDto>> GetAll()
    {
        var modelItems = _userTypesRepo.GetTableNoTracking().ToList();
        var result = _mapper.Map<List<GetUserTypeDto>>(modelItems);

        return ResultHandler.Success(result);
    }

    public async Task<Result<GetUserTypeDto?>> GetById(int id)
    {
        var modelItem = await _userTypesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetUserTypeDto>(modelItem))!;
    }

    public async Task<Result<GetUserTypeDto>> Add(AddUserTypeDto dto)
    {
        var modelItem = _mapper.Map<UserType>(dto);
        _ = await _userTypesRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetUserTypeDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateUserTypeDto dto)
    {
        var modelItem = await _userTypesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        _ = _userTypesRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _userTypesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _userTypesRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}
