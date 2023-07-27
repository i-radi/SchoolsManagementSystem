using Microsoft.AspNetCore.Identity;
using SMS.Models.Entities.Identity;

namespace SMS.Persistance.Seeder;

public static class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<Role> _roleManager)
    {
        using var context = new ApplicationDBContext();
        context.Database.EnsureCreated();

        var rolesCount = await _roleManager.Roles.CountAsync();
        if (rolesCount <= 0)
        {
            await _roleManager.CreateAsync(new Role()
            {
                Name = "SuperAdmin"
            });
            await _roleManager.CreateAsync(new Role()
            {
                Name = "Admin"
            });
            await _roleManager.CreateAsync(new Role()
            {
                Name = "Normal"
            });
        }
    }

}
