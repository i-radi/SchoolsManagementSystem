namespace SMS.Infrastructure.Mapping;

public partial class SeasonProfile
{
    public void GetSeasonByIdMapping()
    {
        CreateMap<Season, GetSeasonDto>()
            .ForMember(dest => dest.School, opt => opt.MapFrom(src => src.School!.Name));
    }
}
