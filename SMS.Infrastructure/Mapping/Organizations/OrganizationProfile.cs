using AutoMapper;

namespace SMS.Infrastructure.Mapping.Organizations;

public partial class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        GetOrganizationByIdMapping();
        AddOrganizationMapping();
        UpdateOrganizationMapping();
    }
}
