using Microsoft.EntityFrameworkCore;
using Models.Entities.Identity;
using Persistance.Context;

namespace Test;

public static class ContextMock
{
    public static ApplicationDBContext Get()
    {
        var builder = new DbContextOptionsBuilder<ApplicationDBContext>();
        var configurationMock = new Mock<IConfiguration>();
        builder.UseInMemoryDatabase("SMS");
        var options = builder.Options;
        var dbContext = new ApplicationDBContext(options, configurationMock.Object);
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