namespace Infrastructure.Mapping;

public partial class ActivityClassroomProfile
{
    public void AddActivityClassroomMapping()
    {
        CreateMap<AddActivityClassroomDto, ActivityClassroom>();
    }
}