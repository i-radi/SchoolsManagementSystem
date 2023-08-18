namespace Infrastructure.Mapping;

public partial class ClassRoomProfile
{
    public void GetClassRoomByIdMapping()
    {
        CreateMap<ClassRoom, GetClassRoomDto>()
            .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade!.Name));

        CreateMap<ClassRoom, ClassRoomViewModel>().ReverseMap();
    }
}
