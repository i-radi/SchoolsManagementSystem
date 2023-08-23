namespace Infrastructure.Mapping;

public partial class ActivityTimeProfile
{
    public void GetActivityTimeByIdMapping()
    {
        CreateMap<ActivityTime, GetActivityTimeDto>();
    }
}
