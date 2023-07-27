namespace SMS.Infrastructure.Mapping;

public partial class SchoolProfile
{
    public void AddSchoolMapping()
    {
        CreateMap<AddSchoolDto, School>();
    }
}