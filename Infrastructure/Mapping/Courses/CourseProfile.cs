namespace Infrastructure.Mapping;

public partial class CourseProfile : Profile
{
    public CourseProfile()
    {
        GetCourseByIdMapping();
        AddCourseMapping();
        UpdateCourseMapping();
    }
}
