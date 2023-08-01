namespace Infrastructure.Mapping;

public partial class GradeProfile
{
    public void UpdateGradeMapping()
    {
        CreateMap<UpdateGradeDto, Grade>();
    }
}
