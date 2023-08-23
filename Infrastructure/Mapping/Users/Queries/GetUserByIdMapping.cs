using Models.Entities.Identity;

namespace Infrastructure.Mapping;

public partial class UserProfile
{
    public void GetUserByIdMapping()
    {
        CreateMap<User, GetUserDto>();
        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => src.Organization!.Name))
            .ReverseMap();
    }
}
