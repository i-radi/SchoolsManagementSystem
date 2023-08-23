namespace Infrastructure.Mapping;

public partial class ActivityClassroomProfile
{
    public void UpdateActivityClassroomMapping()
    {
        CreateMap<UpdateActivityClassroomDto, ActivityClassroom>();
    }
}
