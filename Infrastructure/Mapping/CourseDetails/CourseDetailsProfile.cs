namespace Infrastructure.Mapping;

public partial class CourseDetailsProfile : Profile
{
    public CourseDetailsProfile()
    {
        GetCourseDetailsByIdMapping();
        AddCourseDetailsMapping();
        UpdateCourseDetailsMapping();
    }
}
