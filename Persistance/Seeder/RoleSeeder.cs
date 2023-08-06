using Microsoft.AspNetCore.Identity;
using Models.Entities.Identity;

namespace Persistance.Seeder;

public static class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<Role> _roleManager)
    {
        var rolesCount = await _roleManager.Roles.CountAsync();
        if (rolesCount <= 0)
        {
            await _roleManager.CreateAsync(new Role()
            {
                Name = "SuperAdmin"
            });
            await _roleManager.CreateAsync(new Role()
            {
                Name = "OrganizationAdmin"
            });
            await _roleManager.CreateAsync(new Role()
            {
                Name = "SchoolAdmin"
            });
        }
    }

}
