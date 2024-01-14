namespace Infrastructure.Mapping;

public partial class ActivityTimeProfile
{
    public void AddActivityTimeMapping()
    {
        CreateMap<AddActivityTimeDto, ActivityTime>();
    }
}