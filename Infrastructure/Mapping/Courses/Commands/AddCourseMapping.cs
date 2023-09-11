namespace Infrastructure.Mapping;

public partial class CourseProfile
{
    public void AddCourseMapping()
    {
        CreateMap<AddCourseDto, Course>()
            .ForMember(dest => dest.CourseDetails!.ContentType, opt => opt.MapFrom(src => src.ContentType))
            .ForMember(dest => dest.CourseDetails!.Content, opt => opt.MapFrom(src => src.Content));
    }
}