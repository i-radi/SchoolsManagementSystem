namespace Infrastructure.Mapping;

public partial class SeasonProfile : Profile
{
    public SeasonProfile()
    {
        GetSeasonByIdMapping();
        AddSeasonMapping();
        UpdateSeasonMapping();
    }
}
