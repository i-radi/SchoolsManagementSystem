namespace Infrastructure.Mapping;

public partial class ClassesProfile
{
    public void AddClassMapping()
    {
        CreateMap<AddClassDto, Classes>();
    }
}