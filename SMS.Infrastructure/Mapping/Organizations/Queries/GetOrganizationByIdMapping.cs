namespace SMS.Infrastructure.Mapping;

public partial class OrganizationProfile
{
    public void GetOrganizationByIdMapping()
    {
        CreateMap<Organization, GetOrganizationDto>();
    }
}
