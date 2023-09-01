using Microsoft.AspNetCore.Identity;
using Models.Entities.Identity;

namespace Persistance.Seeder;

public static class UserSeeder
{
    public static async Task SeedAsync(UserManager<User> _userManager, ApplicationDBContext context)
    {
        var usersCount = await _userManager.Users.CountAsync();
        if (usersCount <= 0)
        {
            var defaultuser = new User()
            {
                UserName = "admin",
                Email = "admin@sms.com",
                Name = "Admin User",
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
                ProfilePicturePath = "emptyAvatar.png"
            };
            await _userManager.CreateAsync(defaultuser, "123456");
            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = 1,
                RoleId = 1,
            });
            await context.SaveChangesAsync();
        }
    }
}
