using Models.Entities;

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
                new Activity("First Activity", 1),
                new Activity("Second Activity", 1)
            };

            await dbContext.AddRangeAsync(activities);
            await dbContext.SaveChangesAsync();
        }
    }
}
