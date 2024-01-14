using Models.Entities.Identity;

namespace Core.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepo _userRolesRepo;
    private readonly IMapper _mapper;

    public UserRoleService(IUserRoleRepo userRoleesRepo, IMapper mapper)
    {
        _userRolesRepo = userRoleesRepo;
        _mapper = mapper;
    }

    public Result<PaginatedList<GetUserRoleDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _userRolesRepo.GetTableNoTracking();
        var result = PaginatedList<GetUserRoleDto>.Create(_mapper.Map<List<GetUserRoleDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(result);
    }

    public async Task<Result<bool>> IsExists(AddUserRoleDto model)
    {
        var isExist = await _userRolesRepo.GetTableNoTracking()
            .AnyAsync(ur => ur.UserId == model.UserId
            && ur.RoleId == model.RoleId
            && ur.OrganizationId == model.OrganizationId
            && ur.SchoolId == model.SchoolId
            && ur.ActivityId == model.ActivityId);

        return ResultHandler.Success(isExist);
    }

    public async Task<Result<GetUserRoleDto?>> GetById(int id)
    {
        var modelItem = await _userRolesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetUserRoleDto>(modelItem))!;
    }

    public async Task<Result<GetUserRoleDto>> Add(AddUserRoleDto dto)
    {
        var modelItem = _mapper.Map<UserRole>(dto);

        _ = await _userRolesRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetUserRoleDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateUserRoleDto dto)
    {
        var modelItem = await _userRolesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);
        _ = _userRolesRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(AddUserRoleDto model)
    {
        var dbModel = await _userRolesRepo.GetTableNoTracking()
            .FirstOrDefaultAsync(ur => ur.UserId == model.UserId
            && ur.RoleId == model.RoleId
            && ur.OrganizationId == model.OrganizationId
            && ur.SchoolId == model.SchoolId
            && ur.ActivityId == model.ActivityId);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _userRolesRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}
