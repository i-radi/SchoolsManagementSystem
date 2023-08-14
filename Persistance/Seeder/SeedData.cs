using Microsoft.AspNetCore.Identity;
using Models.Entities.Identity;

namespace Persistance.Seeder;

public static class SeedData
{
    public static async Task SeedAsync(ApplicationDBContext dbContext, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        await OrganizationSeeder.SeedAsync(dbContext);
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
