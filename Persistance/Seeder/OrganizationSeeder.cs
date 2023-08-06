namespace Persistance.Seeder;

public static class OrganizationSeeder
{
    public static async Task SeedAsync(ApplicationDBContext dbContext)
    {
        using var context = new ApplicationDBContext();
        context.Database.EnsureCreated();

        var usersCount = await dbContext.Organizations.CountAsync();
        if (usersCount <= 0)
        {
            var orgs = new List<Organization>
            {
                new Organization{ Name = "Cairo Organization"},
                new Organization{ Name = "Alex Organization"},
                new Organization{ Name = "Tanta Organization"},
            };

            await dbContext.AddRangeAsync(orgs);
            await dbContext.SaveChangesAsync();
        }
    }
}
