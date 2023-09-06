using Microsoft.Extensions.Configuration;

namespace Persistance.Seeder;

public static class OrganizationSeeder
{
    public static async Task SeedAsync(ApplicationDBContext dbContext, IConfiguration configuration)
    {
        var usersCount = await dbContext.Organizations.CountAsync();
        if (usersCount <= 0)
        {
            var orgs = new List<Organization>
            {
                new Organization{ Name = "Cairo Organization"},
            };

            await dbContext.AddRangeAsync(orgs);
            await dbContext.SaveChangesAsync();
        }
    }
}
