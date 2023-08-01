using Models.Entities.Identity;

namespace Infrastructure.Mapping;

public partial class UserProfile
{
    public void UpdateUserMapping()
    {
        CreateMap<UpdateUserDto, User>();
    }
}
