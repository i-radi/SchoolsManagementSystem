namespace Infrastructure.Mapping;

public partial class OrganizationProfile
{
    public void GetOrganizationByIdMapping()
    {
        CreateMap<Organization, GetOrganizationDto>().ReverseMap();
        CreateMap<Organization, OrganizationViewModel>().ReverseMap();
    }
}
