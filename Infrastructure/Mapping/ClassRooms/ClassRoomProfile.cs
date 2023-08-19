using AutoMapper;

namespace Infrastructure.Mapping;

public partial class ClassroomProfile : Profile
{
    public ClassroomProfile()
    {
        GetClassroomByIdMapping();
        AddClassroomMapping();
        UpdateClassroomMapping();
    }
}
