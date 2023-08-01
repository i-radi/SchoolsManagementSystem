using AutoMapper;

namespace Infrastructure.Mapping;

public partial class ClassesProfile : Profile
{
    public ClassesProfile()
    {
        GetClassByIdMapping();
        AddClassMapping();
        UpdateClassMapping();
    }
}
