using Models.Entities;

namespace Persistance.Seeder;

public static class ClassroomSeeder
{
    public static async Task SeedAsync(ApplicationDBContext dbContext)
    {
        var usersCount = await dbContext.ClassRooms.CountAsync();
        if (usersCount <= 0)
        {
            var classrooms = new List<ClassRoom>
            {
                new ClassRoom
                {
                    Name = "Class 1",
                    GradeId = 1,
                },
                new ClassRoom
                {
                    Name = "Class 2",
                    GradeId = 1,
                },
                new ClassRoom
                {
                    Name = "Class 3",
                    GradeId = 1,
                },
            };

            await dbContext.AddRangeAsync(classrooms);
            await dbContext.SaveChangesAsync();
        }
    }
}