namespace Infrastructure.Mapping;

public partial class UserTypeProfile
{
    public void AddUserTypeMapping()
    {
        CreateMap<AddUserTypeDto, UserType>();
    }
}