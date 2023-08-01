using AutoMapper;

namespace Infrastructure.Mapping;

public partial class ClassRoomProfile : Profile
{
    public ClassRoomProfile()
    {
        GetClassRoomByIdMapping();
        AddClassRoomMapping();
        UpdateClassRoomMapping();
    }
}
