using AutoMapper;

namespace Infrastructure.Mapping;

public partial class AuthenticationProfile : Profile
{
    public AuthenticationProfile()
    {
        RegisterMapping();
    }
}

