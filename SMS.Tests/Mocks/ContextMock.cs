using Microsoft.EntityFrameworkCore;
using SMS.Models.Entities.Identity;
using SMS.Persistance.Context;

namespace SMS.Tests;

public static class ContextMock
{
    public static ApplicationDBContext Get()
    {
        var builder = new DbContextOptionsBuilder<ApplicationDBContext>();
        builder.UseInMemoryDatabase("SMS");
        var options = builder.Options;
        var dbContext = new ApplicationDBContext(options);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        SeedData(dbContext);
        return dbContext;
    }

    private static void SeedData(ApplicationDBContext usersContext)
    {
        usersContext.Roles.AddRange(GetRoles());
        usersContext.User.AddRange(GetUsers());

        usersContext.SaveChanges();
    }

    private static IEnumerable<Role> GetRoles()
    {
        return new List<Role>
        {
            new Role{Id = 1, Name = "SuperAdmin"},
            new Role{Id = 2, Name = "Admin"},
            new Role{Id = 3, Name = "Normal"}
        };
    }

    private static IEnumerable<User> GetUsers()
    {
        return new List<User>
        {
            new User
            {
                UserName = "superAdmin",
                Email = "admin@mail.com",
                Name = "organizationProject",
                PlainPassword = "123456"
            },
        };
    }

}