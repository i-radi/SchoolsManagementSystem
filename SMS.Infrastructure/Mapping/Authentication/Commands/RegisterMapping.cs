using SMS.Models.Entities.Identity;

namespace SMS.Infrastructure.Mapping;

public partial class AuthenticationProfile
{
    public void RegisterMapping()
    {
        CreateMap<RegisterDto, User>();
    }
}