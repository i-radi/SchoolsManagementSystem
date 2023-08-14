namespace Persistance.Seeder;

public static class GradeSeeder
{
    public static async Task SeedAsync(ApplicationDBContext dbContext)
    {
        var usersCount = await dbContext.Grades.CountAsync();
        if (usersCount <= 0)
        {
            var grades = new List<Grade>
            {
                new Grade
                {
                    Name = "Grade 1",
                    SchoolId = 1,
                },
                new Grade
                {
                    Name = "Grade 2",
                    SchoolId = 1,
                },
                new Grade
                {
                    Name = "Grade 3",
                    SchoolId = 1,
                },
                new Grade
                {
                    Name = "Grade 4",
                    SchoolId = 1,
                },
            };

            await dbContext.AddRangeAsync(grades);
            await dbContext.SaveChangesAsync();
        }
    }
}
