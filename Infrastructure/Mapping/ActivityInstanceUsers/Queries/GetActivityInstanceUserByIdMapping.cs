namespace Infrastructure.Mapping;

public partial class ActivityInstanceUserProfile
{
    public void GetActivityInstanceUserByIdMapping()
    {
        CreateMap<ActivityInstanceUser, GetActivityInstanceUserDto>();
    }
}
