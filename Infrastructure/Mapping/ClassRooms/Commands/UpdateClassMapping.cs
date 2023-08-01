namespace Infrastructure.Mapping;

public partial class ClassRoomProfile
{
    public void UpdateClassRoomMapping()
    {
        CreateMap<UpdateClassRoomDto, ClassRoom>();
    }
}
