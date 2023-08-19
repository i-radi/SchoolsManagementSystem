namespace Infrastructure.Mapping;

public partial class ClassroomProfile
{
    public void UpdateClassroomMapping()
    {
        CreateMap<UpdateClassroomDto, Classroom>();
    }
}
