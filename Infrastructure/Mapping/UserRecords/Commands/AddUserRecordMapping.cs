namespace Infrastructure.Mapping;

public partial class UserRecordProfile
{
    public void AddUserRecordMapping()
    {
        CreateMap<AddUserRecordDto, UserRecord>();
    }
}