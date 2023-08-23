namespace Infrastructure.Mapping;

public partial class ActivityTimeProfile
{
    public void UpdateActivityTimeMapping()
    {
        CreateMap<UpdateActivityTimeDto, ActivityTime>();
    }
}
