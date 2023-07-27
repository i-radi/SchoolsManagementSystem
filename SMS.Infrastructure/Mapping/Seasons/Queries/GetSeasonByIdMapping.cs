namespace SMS.Infrastructure.Mapping;

public partial class SeasonProfile
{
    public void GetSeasonByIdMapping()
    {
        CreateMap<Season, GetSeasonDto>();
    }
}
