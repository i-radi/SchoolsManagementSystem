namespace SMS.Infrastructure.Mapping;

public partial class OrganizationProfile
{
    public void UpdateOrganizationMapping()
    {
        CreateMap<UpdateOrganizationDto, Organization>();
    }
}
