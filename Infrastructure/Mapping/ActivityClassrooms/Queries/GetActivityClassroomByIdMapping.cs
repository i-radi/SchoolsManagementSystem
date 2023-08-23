namespace Infrastructure.Mapping;

public partial class ActivityClassroomProfile
{
    public void GetActivityClassroomByIdMapping()
    {
        CreateMap<ActivityClassroom, GetActivityClassroomDto>();
    }
}
