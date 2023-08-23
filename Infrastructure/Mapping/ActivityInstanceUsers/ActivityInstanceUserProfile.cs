namespace Infrastructure.Mapping;

public partial class ActivityInstanceUserProfile : Profile
{
    public ActivityInstanceUserProfile()
    {
        GetActivityInstanceUserByIdMapping();
        AddActivityInstanceUserMapping();
        UpdateActivityInstanceUserMapping();
    }
}
