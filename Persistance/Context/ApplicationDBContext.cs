using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Models.Entities;
using Models.Entities.Identity;
using System.Reflection;

namespace Persistance.Context;

public class ApplicationDBContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    #region ctor
    public ApplicationDBContext()
    {
    }
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
    }
    #endregion

    #region DbSets
    public virtual DbSet<User> User => Set<User>();
    public virtual DbSet<Role> Role => Set<Role>();
    public virtual DbSet<UserRole> UserRole => Set<UserRole>();
    public virtual DbSet<Organization> Organizations => Set<Organization>();
    public virtual DbSet<School> Schools => Set<School>();
    public virtual DbSet<Grade> Grades => Set<Grade>();
    public virtual DbSet<ClassRoom> ClassRooms => Set<ClassRoom>();
    public virtual DbSet<Season> Seasons => Set<Season>();
    public virtual DbSet<UserClass> UserClasses => Set<UserClass>();
    public virtual DbSet<UserType> UserTypes => Set<UserType>();
    public virtual DbSet<Activity> Activities => Set<Activity>();
    public virtual DbSet<ActivityClassroom> ActivityClassrooms => Set<ActivityClassroom>();
    public virtual DbSet<ActivityInstance> ActivityInstances => Set<ActivityInstance>();
    public virtual DbSet<ActivityInstanceSeason> ActivityInstanceSeasons => Set<ActivityInstanceSeason>();
    public virtual DbSet<ActivityInstanceUser> ActivityInstanceUsers => Set<ActivityInstanceUser>();
    public virtual DbSet<ActivityTime> ActivityTimes => Set<ActivityTime>();
    #endregion

    #region OnModelCreating & OnConfiguring
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<ActivityInstanceSeason>()
        .HasOne(a => a.Season)
        .WithMany(a => a.ActivityInstanceSeasons)
        .HasForeignKey(a => a.SeasonId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ActivityClassroom>()
        .HasOne(a => a.Classroom)
        .WithMany(a => a.ActivityClassrooms)
        .HasForeignKey(a => a.ClassroomId)
        .OnDelete(DeleteBehavior.Restrict);

        //Change Identity Schema and Table Names
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<UserRole>().ToTable("UserRoles");
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
    #endregion
}
