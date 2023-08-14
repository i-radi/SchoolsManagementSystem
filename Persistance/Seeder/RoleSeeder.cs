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
                Name = "OrganizationAdmin",
                OrganizationId = 1
            });
            await _roleManager.CreateAsync(new Role()
            {
                Name = "SchoolAdmin",
                OrganizationId = 1,
                SchoolId = 1
            });
            await _roleManager.CreateAsync(new Role()
            {
                Name = "FullAccessActivity",
                OrganizationId = 1,
                SchoolId = 1,
                Activity = new Activity("First Activity", 1)
            });
            await _roleManager.CreateAsync(new Role()
            {
                Name = "ReadAccessActivity",
                OrganizationId = 1,
                SchoolId = 1,
                ActivityId = 1
            });
        }
    }

}
