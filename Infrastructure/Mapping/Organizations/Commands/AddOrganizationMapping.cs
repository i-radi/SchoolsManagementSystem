namespace Infrastructure.Mapping;

public partial class OrganizationProfile
{
    public void AddOrganizationMapping()
    {
        CreateMap<AddOrganizationDto, Organization>();
    }
}