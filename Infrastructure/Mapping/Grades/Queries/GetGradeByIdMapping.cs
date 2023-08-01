namespace Infrastructure.Mapping;

public partial class GradeProfile
{
    public void GetGradeByIdMapping()
    {
        CreateMap<Grade, GetGradeDto>()
            .ForMember(dest => dest.School, opt => opt.MapFrom(src => src.School!.Name));
    }
}
