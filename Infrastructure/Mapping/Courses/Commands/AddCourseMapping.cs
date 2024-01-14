namespace Infrastructure.Mapping;

public partial class CourseProfile
{
    public void AddCourseMapping()
    {
        CreateMap<AddCourseDto, Course>();
    }
}