using SMS.Models.Entities.Identity;

namespace SMS.Infrastructure.Mapping;

public partial class UserProfile
{
    public void GetUserByIdMapping()
    {
        CreateMap<User, GetUserDto>();
    }
}
