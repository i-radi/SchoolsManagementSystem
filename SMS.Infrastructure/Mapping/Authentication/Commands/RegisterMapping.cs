using SMS.Models.Entities.Identity;
using SMS.VModels.DTOS.Auth;

namespace SMS.Infrastructure.Mapping.Authentication;

public partial class AuthenticationProfile
{
    public void RegisterMapping()
    {
        CreateMap<RegisterDto, User>();
    }
}