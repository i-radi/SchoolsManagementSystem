using AutoMapper;

namespace Infrastructure.Mapping;

public partial class GradeProfile : Profile
{
    public GradeProfile()
    {
        GetGradeByIdMapping();
        AddGradeMapping();
        UpdateGradeMapping();
    }
}
