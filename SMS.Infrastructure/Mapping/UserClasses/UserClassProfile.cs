using AutoMapper;

namespace SMS.Infrastructure.Mapping;

public partial class UserClassProfile : Profile
{
    public UserClassProfile()
    {
        GetUserClassByIdMapping();
        AddUserClassMapping();
        UpdateUserClassMapping();
    }
}
