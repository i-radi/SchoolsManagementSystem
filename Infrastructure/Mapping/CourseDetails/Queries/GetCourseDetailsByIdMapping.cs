namespace Infrastructure.Mapping;

public partial class CourseDetailsProfile
{
    public void GetCourseDetailsByIdMapping()
    {
        //CreateMap<CourseDetails, GetCourseDetailsDto>();

        CreateMap<CourseDetails, CourseDetailsViewModel>().ReverseMap();
    }
}
