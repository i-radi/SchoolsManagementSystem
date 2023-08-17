namespace Persistance.Seeder;

public static class SeasonSeeder
{
    public static async Task SeedAsync(ApplicationDBContext dbContext)
    {
        var usersCount = await dbContext.Seasons.CountAsync();
        if (usersCount <= 0)
        {
            var seasons = new List<Season>
            {
                new Season
                {
                    Name = "Season 1",
                    From = DateTime.Now.AddMonths(-6),
                    To = DateTime.Now,
                    IsCurrent = true,
                    SchoolId = 1,
                },
                new Season
                {
                    Name = "Season 2",
                    From = DateTime.Now.AddMonths(-12),
                    To = DateTime.Now.AddMonths(-6),
                    IsCurrent = false,
                    SchoolId = 1,
                },
                new Season
                {
                    Name = "Season 3",
                    From = DateTime.Now.AddMonths(-18),
                    To = DateTime.Now.AddMonths(-12),
                    IsCurrent = false,
                    SchoolId = 1,
                },
            };

            await dbContext.AddRangeAsync(seasons);
            await dbContext.SaveChangesAsync();
        }
    }
}
