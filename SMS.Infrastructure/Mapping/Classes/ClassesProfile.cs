using AutoMapper;

namespace SMS.Infrastructure.Mapping;

public partial class ClassesProfile : Profile
{
    public ClassesProfile()
    {
        GetClassByIdMapping();
        AddClassMapping();
        UpdateClassMapping();
    }
}
