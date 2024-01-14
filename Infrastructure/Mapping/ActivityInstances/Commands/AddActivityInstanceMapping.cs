namespace Infrastructure.Mapping;

public partial class ActivityInstanceProfile
{
    public void AddActivityInstanceMapping()
    {
        CreateMap<AddActivityInstanceDto, ActivityInstance>();
    }
}