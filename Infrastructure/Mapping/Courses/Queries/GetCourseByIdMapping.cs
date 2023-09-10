namespace Infrastructure.Mapping;

public partial class CourseProfile
{
    public void GetCourseByIdMapping()
    {
        //CreateMap<Course, GetCourseDto>();

        CreateMap<Course, CourseViewModel>().ReverseMap();
    }
}
