namespace Infrastructure.Mapping;

public partial class CourseProfile
{
    public void GetCourseByIdMapping()
    {
        //CreateMap<Course, GetCourseDto>();

        CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.CourseDetails!.ContentType))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.CourseDetails!.Content))
            .ReverseMap();
    }
}
