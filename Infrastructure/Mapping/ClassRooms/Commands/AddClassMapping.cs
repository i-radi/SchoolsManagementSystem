namespace Infrastructure.Mapping;

public partial class ClassRoomProfile
{
    public void AddClassRoomMapping()
    {
        CreateMap<AddClassRoomDto, ClassRoom>();
    }
}