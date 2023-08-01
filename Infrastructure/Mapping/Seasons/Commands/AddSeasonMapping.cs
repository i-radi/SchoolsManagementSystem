namespace Infrastructure.Mapping;

public partial class SeasonProfile
{
    public void AddSeasonMapping()
    {
        CreateMap<AddSeasonDto, Season>();
    }
}