namespace Infrastructure.Mapping;

public partial class ClassesProfile
{
    public void GetClassByIdMapping()
    {
        CreateMap<Classes, GetClassDto>()
            .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade!.Name));
    }
}
