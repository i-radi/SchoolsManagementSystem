namespace Infrastructure.Mapping;

public partial class ActivityInstanceUserProfile
{
    public void AddActivityInstanceUserMapping()
    {
        CreateMap<AddActivityInstanceUserDto, ActivityInstanceUser>();
    }
}