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
                     Name = "First Activity",
                     SchoolId = 1,
                     Order=3,
                     ForStudents = true,
                     IsAvailable = true,
                     Location = "st. 1"
                },
                new Activity
                {
                     Name = "Second Activity",
                     SchoolId = 1,
                     Order=1,
                     ForTeachers = true,
                     IsAvailable = true,
                     Location = "st. 1"
                }
            };

            await dbContext.AddRangeAsync(activities);
            await dbContext.SaveChangesAsync();
        }
    }
}
