using Models.Entities.Identity;

namespace Infrastructure.Mapping;

public partial class UserRoleProfile
{
    public void AddUserRoleMapping()
    {
        CreateMap<AddUserRoleDto, UserRole>();
    }
}