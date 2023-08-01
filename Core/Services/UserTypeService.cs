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

    public Response<List<GetUserTypeDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _userTypesRepo.GetTableNoTracking();
        var result = PaginatedList<GetUserTypeDto>.Create(_mapper.Map<List<GetUserTypeDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetUserTypeDto>>(result));
    }

    public async Task<Response<GetUserTypeDto?>> GetById(int id)
    {
        var modelItem = await _userTypesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetUserTypeDto>(modelItem))!;
    }

    public async Task<Response<GetUserTypeDto>> Add(AddUserTypeDto dto)
    {
        var modelItem = _mapper.Map<UserType>(dto);

        var model = await _userTypesRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetUserTypeDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateUserTypeDto dto)
    {
        var modelItem = await _userTypesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _userTypesRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _userTypesRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _userTypesRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
