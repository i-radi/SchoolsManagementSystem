using SMS.Models.Entities.Identity;
using SMS.VModels.DTOS.Users.Queries;

namespace SMS.Infrastructure.Mapping.Users;

public partial class UserProfile
{
    public void GetUserByIdMapping()
    {
        CreateMap<User, GetUserDto>();
    }
}
