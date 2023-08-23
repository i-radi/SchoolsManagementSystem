namespace Infrastructure.Mapping;

public partial class ActivityInstanceUserProfile
{
    public void UpdateActivityInstanceUserMapping()
    {
        CreateMap<UpdateActivityInstanceUserDto, ActivityInstanceUser>();
    }
}
