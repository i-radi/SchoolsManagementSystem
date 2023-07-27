using SMS.Models.Entities.Identity;
using SMS.VModels.DTOS.Auth;

namespace SMS.Infrastructure.Mapping;

public partial class AuthenticationProfile
{
    public void RegisterMapping()
    {
        CreateMap<RegisterDto, User>();
    }
}