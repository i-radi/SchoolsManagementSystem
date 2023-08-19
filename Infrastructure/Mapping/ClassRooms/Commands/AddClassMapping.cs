namespace Infrastructure.Mapping;

public partial class ClassroomProfile
{
    public void AddClassroomMapping()
    {
        CreateMap<AddClassroomDto, Classroom>();
    }
}