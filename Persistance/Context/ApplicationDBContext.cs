using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Models.Entities.Identity;
using System.Reflection;

namespace Persistance.Context;

public class ApplicationDBContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public ApplicationDBContext()
    {
    }
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
    }
    public virtual DbSet<User> User => Set<User>();
    public virtual DbSet<Organization> Organizations => Set<Organization>();
    public virtual DbSet<School> Schools => Set<School>();
    public virtual DbSet<Grade> Grades => Set<Grade>();
    public virtual DbSet<ClassRoom> ClassRooms => Set<ClassRoom>();
    public virtual DbSet<Season> Seasons => Set<Season>();
    public virtual DbSet<UserClass> UserClasses => Set<UserClass>();
    public virtual DbSet<UserType> UserTypes => Set<UserType>();
    public virtual DbSet<Activity> Activities => Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //Change Identity Schema and Table Names
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=SMS;Integrated Security=True ;TrustServerCertificate=True");
        }
    }
}
