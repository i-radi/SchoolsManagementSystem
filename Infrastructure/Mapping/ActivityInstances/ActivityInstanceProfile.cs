namespace Infrastructure.Mapping;

public partial class ActivityInstanceProfile : Profile
{
    public ActivityInstanceProfile()
    {
        GetActivityInstanceByIdMapping();
        AddActivityInstanceMapping();
        UpdateActivityInstanceMapping();
    }
}
