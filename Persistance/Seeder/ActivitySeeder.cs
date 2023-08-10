namespace Persistance.Seeder;

public static class ActivitySeeder
{
    public static async Task SeedAsync(ApplicationDBContext dbContext)
    {
        var activitiesCount = await dbContext.Activities.CountAsync();
        if (activitiesCount <= 0)
        {
            var activities = new List<Activity>
            {
                new Activity
                {
                     Title = "First Activity",
                     SchoolId = 1,
                     Role = new Models.Entities.Identity.Role
                     {
                         Name = "FirstActivity@Cairo1School",
                         NormalizedName = "FirstActivity@Cairo1School".ToUpper(),
                     }
                },
                new Activity
                {
                     Title = "Second Activity",
                     SchoolId = 1,
                     Role = new Models.Entities.Identity.Role
                     {
                         Name = "SecondActivity@Cairo1School",
                         NormalizedName = "SecondActivity@Cairo1School".ToUpper(),
                     }
                }
            };

            await dbContext.AddRangeAsync(activities);
            await dbContext.SaveChangesAsync();
        }
    }
}
