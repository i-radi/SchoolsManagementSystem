using Models.Entities.Identity;

namespace Infrastructure.Mapping;

public partial class UserRoleProfile
{
    public void GetUserRoleByIdMapping()
    {
        CreateMap<UserRole, GetUserRoleDto>();
    }
}
