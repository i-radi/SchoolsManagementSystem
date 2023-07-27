namespace SMS.Infrastructure.Mapping;

public partial class SeasonProfile
{
    public void UpdateSeasonMapping()
    {
        CreateMap<UpdateSeasonDto, Season>();
    }
}
