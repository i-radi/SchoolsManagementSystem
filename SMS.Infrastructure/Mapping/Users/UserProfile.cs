using AutoMapper;

namespace SMS.Infrastructure.Mapping.Users;

public partial class UserProfile : Profile
{
    public UserProfile()
    {
        GetUserByIdMapping();
        AddUserMapping();
        UpdateUserMapping();
    }
}
