namespace Infrastructure.Mapping;

public partial class UserRecordProfile : Profile
{
    public UserRecordProfile()
    {
        GetUserRecordByIdMapping();
        AddUserRecordMapping();
        UpdateUserRecordMapping();
    }
}
