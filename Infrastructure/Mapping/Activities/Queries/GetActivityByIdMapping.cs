using Models.Entities;

namespace Infrastructure.Mapping;

public partial class ActivityProfile
{
    public void GetActivityByIdMapping()
    {
        CreateMap<Activity, GetActivityDto>()
            .ForMember(dest => dest.School, opt => opt.MapFrom(src => src.School!.Name));
    }
}
