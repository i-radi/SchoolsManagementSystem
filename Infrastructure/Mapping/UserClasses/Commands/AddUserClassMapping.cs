namespace Infrastructure.Mapping;

public partial class UserClassProfile
{
    public void AddUserClassMapping()
    {
        CreateMap<AddUserClassDto, UserClass>();
    }
}