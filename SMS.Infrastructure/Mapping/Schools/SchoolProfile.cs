using AutoMapper;

namespace SMS.Infrastructure.Mapping;

public partial class SchoolProfile : Profile
{
    public SchoolProfile()
    {
        GetSchoolByIdMapping();
        AddSchoolMapping();
        UpdateSchoolMapping();
    }
}
