namespace SMS.Infrastructure.Mapping;

public partial class SchoolProfile
{
    public void UpdateSchoolMapping()
    {
        CreateMap<UpdateSchoolDto, School>();
    }
}
