using SMS.Models.Entities;
using SMS.Models.Entities.Identity;
using SMS.VModels.DTOS.Organizations.Commands;
using SMS.VModels.DTOS.Users.Commands;

namespace SMS.Infrastructure.Mapping.Organizations;

public partial class OrganizationProfile
{
    public void UpdateOrganizationMapping()
    {
        CreateMap<UpdateOrganizationDto, Organization>();
    }
}
