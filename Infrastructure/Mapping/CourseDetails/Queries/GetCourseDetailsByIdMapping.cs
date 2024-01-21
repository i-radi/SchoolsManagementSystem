namespace Infrastructure.Mapping;

public partial class CourseDetailsProfile
{
    public void GetCourseDetailsByIdMapping()
    {
        CreateMap<CourseDetails, CourseDetailsViewModel>().ReverseMap();
    }
}
