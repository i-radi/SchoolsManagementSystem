using AutoMapper;

namespace SMS.Infrastructure.Mapping;

public partial class UserProfile : Profile
{
    public UserProfile()
    {
        GetUserByIdMapping();
        AddUserMapping();
        UpdateUserMapping();
    }
}
