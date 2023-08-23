namespace Infrastructure.Mapping;

public partial class ActivityClassroomProfile : Profile
{
    public ActivityClassroomProfile()
    {
        GetActivityClassroomByIdMapping();
        AddActivityClassroomMapping();
        UpdateActivityClassroomMapping();
    }
}
