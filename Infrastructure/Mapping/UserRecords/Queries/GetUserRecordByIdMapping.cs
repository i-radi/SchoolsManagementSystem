namespace Infrastructure.Mapping;

public partial class UserRecordProfile
{
    public void GetUserRecordByIdMapping()
    {
        //CreateMap<UserRecord, GetUserRecordDto>();

        CreateMap<UserRecord, UserRecordViewModel>().ReverseMap();
    }
}
