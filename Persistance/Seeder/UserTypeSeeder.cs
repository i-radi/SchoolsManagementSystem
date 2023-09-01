namespace Persistance.Seeder;

public static class UserTypeSeeder
{
    public static async Task SeedAsync(ApplicationDBContext dbContext)
    {
        var usersCount = await dbContext.UserTypes.CountAsync();
        if (usersCount <= 0)
        {
            var userTypes = new List<UserType>
            {
                new UserType{ Name = "Teacher"},
                new UserType{ Name = "Student"},
                new UserType{ Name = "Parent"},
            };

            await dbContext.AddRangeAsync(userTypes);
            await dbContext.SaveChangesAsync();
        }
    }
}
