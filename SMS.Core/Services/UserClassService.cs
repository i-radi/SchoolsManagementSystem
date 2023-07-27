using Microsoft.EntityFrameworkCore;

namespace SMS.Core.Services;

public class UserClassService : IUserClassService
{
    private readonly IUserClassRepo _userClassRepo;
    private readonly IMapper _mapper;

    public UserClassService(IUserClassRepo userClassRepo, IMapper mapper)
    {
        _userClassRepo = userClassRepo;
        _mapper = mapper;
    }

    public List<GetUserClassDto> GetAll()
    {
        var modelItems = _userClassRepo
            .GetTableNoTracking()
            .Include(m => m.User)
            .Include(m => m.Classes)
            .Include(m => m.UserType)
            .Include(m => m.Season);

        return _mapper.Map<List<GetUserClassDto>>(modelItems);
    }

    public async Task<GetUserClassDto?> GetById(int id)
    {
        var modelItem = await _userClassRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return _mapper.Map<GetUserClassDto>(modelItem);
    }

    public async Task<GetUserClassDto> Add(AddUserClassDto dto)
    {
        var modelItem = _mapper.Map<UserClass>(dto);

        var model = await _userClassRepo.AddAsync(modelItem);
        await _userClassRepo.SaveChangesAsync();

        return _mapper.Map<GetUserClassDto>(modelItem);
    }

    public async Task<bool> Update(UpdateUserClassDto dto)
    {
        var modelItem = await _userClassRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return false;

        _mapper.Map(dto, modelItem);

        var model = _userClassRepo.UpdateAsync(modelItem);
        await _userClassRepo.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(int id)
    {

        var dbModel = await _userClassRepo.GetByIdAsync(id);

        if (dbModel == null)
            return false;

        await _userClassRepo.DeleteAsync(dbModel);
        await _userClassRepo.SaveChangesAsync();
        return true;
    }
}
