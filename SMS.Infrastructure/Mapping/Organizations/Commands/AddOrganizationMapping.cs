using SMS.Models.Entities;
using SMS.VModels.DTOS.Organizations.Commands;

namespace SMS.Infrastructure.Mapping.Organizations;

public partial class OrganizationProfile
{
    public void AddOrganizationMapping()
    {
        CreateMap<AddClassDto, Organization>();
    }
}