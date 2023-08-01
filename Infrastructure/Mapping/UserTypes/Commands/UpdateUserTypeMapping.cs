namespace Infrastructure.Mapping;

public partial class UserTypeProfile
{
    public void UpdateUserTypeMapping()
    {
        CreateMap<UpdateUserTypeDto, UserType>();
    }
}
