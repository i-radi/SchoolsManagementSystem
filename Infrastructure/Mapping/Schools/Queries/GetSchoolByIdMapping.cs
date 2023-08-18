namespace Infrastructure.Mapping;

public partial class SchoolProfile
{
    public void GetSchoolByIdMapping()
    {
        CreateMap<School, GetSchoolDto>()
            .ForMember(dest => dest.Organization, opt => opt.MapFrom(src => src.Organization!.Name));
        CreateMap<School, SchoolViewModel>().ReverseMap();
    }
}
