using AutoMapper;

namespace Infrastructure.Mapping;

public partial class UserTypeProfile : Profile
{
    public UserTypeProfile()
    {
        GetUserTypeByIdMapping();
        AddUserTypeMapping();
        UpdateUserTypeMapping();
    }
}
