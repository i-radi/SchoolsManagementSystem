namespace Infrastructure.Mapping;

public partial class ActivityProfile
{
    public void GetActivityByIdMapping()
    {
        CreateMap<Activity, GetActivityDto>()
            .ForMember(dest => dest.School, opt => opt.MapFrom(src => src.School!.Name));

        CreateMap<Activity, ActivityViewModel>().ReverseMap();
        CreateMap<ActivityTime, ActivityTimeViewModel>().ReverseMap();
        CreateMap<ActivityInstance, ActivityInstanceViewModel>().ReverseMap();
        CreateMap<ActivityClassroom, ActivityClassroomViewModel>().ReverseMap();
        CreateMap<ActivityInstanceUser, ActivityInstanceUserViewModel>().ReverseMap();
    }
}
