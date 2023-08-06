namespace Persistance.Seeder;

public static class SchoolSeeder
{
    public static async Task SeedAsync(ApplicationDBContext dbContext)
    {
        var usersCount = await dbContext.UserTypes.CountAsync();
        if (usersCount <= 0)
        {
            var schools = new List<School>
            {
                new School
                {
                    Name = "Cairo 1 School ",
                    Description = "desc. of Cairo 1 School",
                    OrganizationId = 1,
                },
                new School
                {
                    Name = "Cairo 2 School ",
                    Description = "desc. of Cairo 2 School",
                    OrganizationId = 1,
                },
                new School
                {
                    Name = "Alex 1 School ",
                    Description = "desc. of Alex 1 School",
                    OrganizationId = 2,
                },
                new School
                {
                    Name = "Alex 2 School ",
                    Description = "desc. of Alex 2 School",
                    OrganizationId = 2,
                }
            };

            await dbContext.AddRangeAsync(schools);
            await dbContext.SaveChangesAsync();
        }
    }
}
