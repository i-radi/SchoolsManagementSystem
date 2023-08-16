using Models.Entities;

namespace Infrastructure.Mapping;

public partial class ActivityProfile
{
    public void UpdateActivityMapping()
    {
        CreateMap<UpdateActivityDto, Activity>();
    }
}
