namespace Infrastructure.Mapping;

public partial class ActivityInstanceProfile
{
    public void UpdateActivityInstanceMapping()
    {
        CreateMap<UpdateActivityInstanceDto, ActivityInstance>();
    }
}
