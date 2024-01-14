namespace Infrastructure.Mapping;

public partial class UserRoleProfile : Profile
{
    public UserRoleProfile()
    {
        GetUserRoleByIdMapping();
        AddUserRoleMapping();
        UpdateUserRoleMapping();
    }
}
