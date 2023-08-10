using AutoMapper;

namespace Infrastructure.Mapping;

public partial class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        GetActivityByIdMapping();
        AddActivityMapping();
        UpdateActivityMapping();
    }
}
