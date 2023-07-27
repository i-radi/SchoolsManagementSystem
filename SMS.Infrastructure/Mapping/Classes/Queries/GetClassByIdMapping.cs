namespace SMS.Infrastructure.Mapping;

public partial class ClassesProfile
{
    public void GetClassByIdMapping()
    {
        CreateMap<Classes, GetClassDto>();
    }
}
