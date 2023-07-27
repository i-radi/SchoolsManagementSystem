namespace SMS.Infrastructure.Mapping;

public partial class ClassesProfile
{
    public void UpdateClassMapping()
    {
        CreateMap<UpdateClassDto, Classes>();
    }
}
