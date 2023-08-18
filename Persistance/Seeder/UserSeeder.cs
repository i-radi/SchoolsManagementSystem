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
                UserName = "admin@mail.com",
                Email = "admin@mail.com",
                Name = "El kbeer",
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

            var cairoOrgUser = new User()
            {
                UserName = "CairoAdmin@mail.com",
                Email = "CairoAdmin@mail.com",
                Name = "Kbeer Cairo",
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
                ProfilePicturePath = "emptyAvatar.png"
            };
            await _userManager.CreateAsync(cairoOrgUser, "123456");
            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = 2,
                RoleId = 2,
                OrganizationId = 1
            });

            var alexOrgUser = new User()
            {
                UserName = "AlexAdmin@mail.com",
                Email = "AlexAdmin@mail.com",
                Name = "Kbeer Alex",
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
                ProfilePicturePath = "emptyAvatar.png"
            };
            await _userManager.CreateAsync(alexOrgUser, "123456");
            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = 3,
                RoleId = 2,
                OrganizationId = 2,
            });

            var tantaOrgUser = new User()
            {
                UserName = "TantaAdmin@mail.com",
                Email = "TantaAdmin@mail.com",
                Name = "Kbeer Tanta",
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
                ProfilePicturePath = "emptyAvatar.png"
            };
            await _userManager.CreateAsync(tantaOrgUser, "123456");
            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = 4,
                RoleId = 2,
                OrganizationId = 3,
            });

            var cairo1SchoolUser = new User()
            {
                UserName = "cairo1@mail.com",
                Email = "cairo1@mail.com",
                Name = "Cairo School 1 Admin",
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
                ProfilePicturePath = "emptyAvatar.png"
            };
            await _userManager.CreateAsync(cairo1SchoolUser, "123456");
            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = 5,
                RoleId = 3,
                OrganizationId = 1,
                SchoolId = 1
            });

            var cairo2SchoolUser = new User()
            {
                UserName = "cairo2@mail.com",
                Email = "cairo2@mail.com",
                Name = "Cairo School 2 Admin",
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
                ProfilePicturePath = "emptyAvatar.png"
            };
            await _userManager.CreateAsync(cairo2SchoolUser, "123456");
            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = 6,
                RoleId = 3,
                OrganizationId = 1,
                SchoolId = 2
            });

            var alex1SchoolUser = new User()
            {
                UserName = "alex1@mail.com",
                Email = "alex1@mail.com",
                Name = "Alex School 1 Admin",
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
                ProfilePicturePath = "emptyAvatar.png"
            };
            await _userManager.CreateAsync(alex1SchoolUser, "123456");
            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = 7,
                RoleId = 3,
                OrganizationId = 2,
                SchoolId = 3
            });

            var alex2SchoolUser = new User()
            {
                UserName = "alex2@mail.com",
                Email = "alex2@mail.com",
                Name = "Alex School 2 Admin",
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
                ProfilePicturePath = "emptyAvatar.png"
            };
            await _userManager.CreateAsync(alex2SchoolUser, "123456");
            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = 8,
                RoleId = 3,
                OrganizationId = 2,
                SchoolId = 4
            });

            var userWithoutRole = new User()
            {
                UserName = "tanta@mail.com",
                Email = "tanta@mail.com",
                Name = "Tanta School  Admin",
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
                ProfilePicturePath = "emptyAvatar.png"
            };
            await _userManager.CreateAsync(userWithoutRole, "123456");

            await context.SaveChangesAsync();
        }
    }
}
