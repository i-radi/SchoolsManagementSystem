using SMS.Models.Entities;
using SMS.VModels.DTOS.Organizations.Queries;

namespace SMS.Infrastructure.Mapping.Organizations;

public partial class OrganizationProfile
{
    public void GetOrganizationByIdMapping()
    {
        CreateMap<Organization, GetClassDto>();
    }
}
