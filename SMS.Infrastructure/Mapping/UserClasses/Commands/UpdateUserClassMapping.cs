namespace SMS.Infrastructure.Mapping;

public partial class UserClassProfile
{
    public void UpdateUserClassMapping()
    {
        CreateMap<UpdateUserClassDto, UserClass>();
    }
}
