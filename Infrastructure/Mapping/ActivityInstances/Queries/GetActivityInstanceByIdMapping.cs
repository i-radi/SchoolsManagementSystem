namespace Infrastructure.Mapping;

public partial class ActivityInstanceProfile
{
    public void GetActivityInstanceByIdMapping()
    {
        CreateMap<ActivityInstance, GetActivityInstanceDto>();
    }
}
