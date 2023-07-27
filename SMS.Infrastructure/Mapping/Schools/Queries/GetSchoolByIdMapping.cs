namespace SMS.Infrastructure.Mapping;

public partial class SchoolProfile
{
    public void GetSchoolByIdMapping()
    {
        CreateMap<School, GetSchoolDto>();
    }
}
