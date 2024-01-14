namespace Infrastructure.Mapping;

public partial class ActivityTimeProfile : Profile
{
    public ActivityTimeProfile()
    {
        GetActivityTimeByIdMapping();
        AddActivityTimeMapping();
        UpdateActivityTimeMapping();
    }
}
