using AutoMapper;

namespace Infrastructure.Mapping;

public partial class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        GetOrganizationByIdMapping();
        AddOrganizationMapping();
        UpdateOrganizationMapping();
    }
}
