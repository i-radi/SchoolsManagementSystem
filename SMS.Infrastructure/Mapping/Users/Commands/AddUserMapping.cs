using SMS.Models.Entities.Identity;
using SMS.VModels.DTOS.Users.Commands;

namespace SMS.Infrastructure.Mapping.Users;

public partial class UserProfile
{
    public void AddUserMapping()
    {
        CreateMap<AddUserDto, User>();
    }
}