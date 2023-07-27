using AutoMapper;

namespace SMS.Infrastructure.Mapping;

public partial class AuthenticationProfile : Profile
{
    public AuthenticationProfile()
    {
        RegisterMapping();
    }
}

