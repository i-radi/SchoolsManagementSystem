using AutoMapper;

namespace SMS.Infrastructure.Mapping;

public partial class UserTypeProfile : Profile
{
    public UserTypeProfile()
    {
        GetUserTypeByIdMapping();
        AddUserTypeMapping();
        UpdateUserTypeMapping();
    }
}
