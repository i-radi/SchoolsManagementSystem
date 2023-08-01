using Models.Entities.Identity;

namespace Infrastructure.Mapping;

public partial class AuthenticationProfile
{
    public void RegisterMapping()
    {
        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.PlainPassword, opt => opt.MapFrom(src => src.Password));
    }
}