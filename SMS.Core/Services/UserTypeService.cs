namespace SMS.Core.Services;

public class UserTypeService : IUserTypeService
{
    private readonly IUserTypeRepo _userTypeRepo;
    private readonly IMapper _mapper;

    public UserTypeService(IUserTypeRepo userTypeRepo, IMapper mapper)
    {
        _userTypeRepo = userTypeRepo;
        _mapper = mapper;
    }

    public List<GetUserTypeDto> GetAll()
    {
        var modelItems = _userTypeRepo.GetTableNoTracking();

        return _mapper.Map<List<GetUserTypeDto>>(modelItems);
    }

    public async Task<GetUserTypeDto?> GetById(int id)
    {
        var modelItem = await _userTypeRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return _mapper.Map<GetUserTypeDto>(modelItem);
    }

    public async Task<GetUserTypeDto> Add(AddUserTypeDto dto)
    {
        var modelItem = _mapper.Map<UserType>(dto);

        var model = await _userTypeRepo.AddAsync(modelItem);
        await _userTypeRepo.SaveChangesAsync();

        return _mapper.Map<GetUserTypeDto>(modelItem);
    }

    public async Task<bool> Update(UpdateUserTypeDto dto)
    {
        var modelItem = await _userTypeRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return false;

        _mapper.Map(dto, modelItem);

        var model = _userTypeRepo.UpdateAsync(modelItem);
        await _userTypeRepo.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(int id)
    {

        var dbModel = await _userTypeRepo.GetByIdAsync(id);

        if (dbModel == null)
            return false;

        await _userTypeRepo.DeleteAsync(dbModel);
        await _userTypeRepo.SaveChangesAsync();
        return true;
    }
}
