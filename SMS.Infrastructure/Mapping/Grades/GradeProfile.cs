using AutoMapper;

namespace SMS.Infrastructure.Mapping;

public partial class GradeProfile : Profile
{
    public GradeProfile()
    {
        GetGradeByIdMapping();
        AddGradeMapping();
        UpdateGradeMapping();
    }
}
