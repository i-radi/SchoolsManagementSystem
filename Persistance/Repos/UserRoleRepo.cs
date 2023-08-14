using Models.Entities.Identity;

namespace Persistance.Repos;

public class UserRoleRepo : GenericRepoAsync<UserRole>, IUserRoleRepo
{
    #region Fields
    private DbSet<UserRole> userRoles;
    #endregion

    #region Constructors
    public UserRoleRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        userRoles = dbContext.Set<UserRole>();
    }
    #endregion

    #region Handle Methods

    #endregion
}