using Models.Entities.Identity;

namespace Infrastructure.Mapping;

public partial class UserProfile
{
    public void GetUserByIdMapping()
    {
        CreateMap<User, GetUserDto>();
        CreateMap<User, GetProfileDto>();
        CreateMap<User, UserViewModel>()
            .ReverseMap();
    }
}
