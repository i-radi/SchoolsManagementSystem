using AutoMapper;

namespace SMS.Infrastructure.Mapping.Authentication;

public partial class AuthenticationProfile : Profile
{
    public AuthenticationProfile()
    {
        RegisterMapping();
    }
}

