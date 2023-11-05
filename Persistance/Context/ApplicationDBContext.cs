using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Entities.Identity;
using System.Reflection;

namespace Persistance.Context;

public class ApplicationDBContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    #region feilds
    private readonly IConfiguration _configuration;
    #endregion

    #region ctor
    public ApplicationDBContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
    #endregion

    #region DbSets
    public virtual DbSet<User> User => Set<User>();
    public virtual DbSet<Role> Role => Set<Role>();
    public virtual DbSet<UserRole> UserRole => Set<UserRole>();
    public virtual DbSet<Organization> Organizations => Set<Organization>();
    public virtual DbSet<UserOrganization> UserOrganizations => Set<UserOrganization>();
    public virtual DbSet<School> Schools => Set<School>();
    public virtual DbSet<Grade> Grades => Set<Grade>();
    public virtual DbSet<Classroom> Classrooms => Set<Classroom>();
    public virtual DbSet<Season> Seasons => Set<Season>();
    public virtual DbSet<UserClass> UserClasses => Set<UserClass>();
    public virtual DbSet<UserType> UserTypes => Set<UserType>();
    public virtual DbSet<Activity> Activities => Set<Activity>();
    public virtual DbSet<ActivityClassroom> ActivityClassrooms => Set<ActivityClassroom>();
    public virtual DbSet<ActivityInstance> ActivityInstances => Set<ActivityInstance>();
    public virtual DbSet<ActivityInstanceUser> ActivityInstanceUsers => Set<ActivityInstanceUser>();
    public virtual DbSet<ActivityTime> ActivityTimes => Set<ActivityTime>();
    public virtual DbSet<Record> Records => Set<Record>();
    public virtual DbSet<UserRecord> UserRecords => Set<UserRecord>();
    public virtual DbSet<RecordClass> RecordClasses => Set<RecordClass>();
    public virtual DbSet<Course> Courses => Set<Course>();
    public virtual DbSet<CourseDetails> CourseDetails => Set<CourseDetails>();
    #endregion

    #region OnModelCreating & OnConfiguring
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        ChangeIdentitSchemaAndTableNames(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = _configuration.GetConnectionString("ApplicationDbContextConnection")!;
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    #endregion

    #region PrivateMethods

    private static void ChangeIdentitSchemaAndTableNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<UserRole>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
    }

    #endregion
}
