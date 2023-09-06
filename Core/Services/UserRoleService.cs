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

    public Response<List<GetUserRoleDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _userRolesRepo.GetTableNoTracking();
        var result = PaginatedList<GetUserRoleDto>.Create(_mapper.Map<List<GetUserRoleDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetUserRoleDto>>(result));
    }

    public async Task<Response<bool>> IsExists(AddUserRoleDto model)
    {
        var isExist = await _userRolesRepo.GetTableNoTracking()
            .AnyAsync(ur => ur.UserId == model.UserId
            && ur.RoleId == model.RoleId
            && ur.OrganizationId == model.OrganizationId
            && ur.SchoolId == model.SchoolId
            && ur.ActivityId == model.ActivityId);

        return ResponseHandler.Success(isExist);
    }

    public async Task<Response<GetUserRoleDto?>> GetById(int id)
    {
        var modelItem = await _userRolesRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetUserRoleDto>(modelItem))!;
    }

    public async Task<Response<GetUserRoleDto>> Add(AddUserRoleDto dto)
    {
        var modelItem = _mapper.Map<UserRole>(dto);

        var model = await _userRolesRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetUserRoleDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateUserRoleDto dto)
    {
        var modelItem = await _userRolesRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _userRolesRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(AddUserRoleDto model)
    {
        var dbModel = await _userRolesRepo.GetTableNoTracking()
            .FirstOrDefaultAsync(ur => ur.UserId == model.UserId
            && ur.RoleId == model.RoleId
            && ur.OrganizationId == model.OrganizationId
            && ur.SchoolId == model.SchoolId
            && ur.ActivityId == model.ActivityId);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _userRolesRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
