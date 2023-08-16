namespace Infrastructure.Mapping;

public partial class ActivityProfile
{
    public void AddActivityMapping()
    {
        CreateMap<AddActivityDto, Activity>();
    }
}