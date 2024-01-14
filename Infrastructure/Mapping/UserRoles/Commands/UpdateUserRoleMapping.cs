using Models.Entities.Identity;

namespace Infrastructure.Mapping;

public partial class UserRoleProfile
{
    public void UpdateUserRoleMapping()
    {
        CreateMap<UpdateUserRoleDto, UserRole>();
    }
}
