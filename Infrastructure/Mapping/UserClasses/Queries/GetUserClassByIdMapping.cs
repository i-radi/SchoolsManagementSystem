namespace Infrastructure.Mapping;

public partial class UserClassProfile
{
    public void GetUserClassByIdMapping()
    {
        CreateMap<UserClass, GetUserClassDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User!.Name))
            .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.Classes!.Name))
            .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType!.Name))
            .ForMember(dest => dest.Season, opt => opt.MapFrom(src => src.Season!.From.ToShortDateString()));
    }
}
