namespace SMS.Infrastructure.Mapping;

public partial class GradeProfile
{
    public void AddGradeMapping()
    {
        CreateMap<AddGradeDto, Grade>();
    }
}