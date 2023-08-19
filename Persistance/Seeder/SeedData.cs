using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models.Entities.Identity;

namespace Persistance.Seeder;

public static class SeedData
{
    public static async Task SeedAsync(
        ApplicationDBContext dbContext,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IConfiguration configuration)
    {
        await OrganizationSeeder.SeedAsync(dbContext, configuration);
        await SchoolSeeder.SeedAsync(dbContext);
        await UserTypeSeeder.SeedAsync(dbContext);
        await GradeSeeder.SeedAsync(dbContext);
        await SeasonSeeder.SeedAsync(dbContext);
        await ClassroomSeeder.SeedAsync(dbContext);
        await RoleSeeder.SeedAsync(roleManager);
        await UserSeeder.SeedAsync(userManager, dbContext);
        await ActivitySeeder.SeedAsync(dbContext);
    }
}
