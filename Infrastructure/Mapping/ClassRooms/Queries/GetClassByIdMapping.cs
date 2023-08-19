namespace Infrastructure.Mapping;

public partial class ClassroomProfile
{
    public void GetClassroomByIdMapping()
    {
        CreateMap<Classroom, GetClassroomDto>()
            .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade!.Name));

        CreateMap<Classroom, ClassroomViewModel>().ReverseMap();
    }
}
