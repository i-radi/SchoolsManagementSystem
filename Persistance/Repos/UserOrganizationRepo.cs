using Models.Entities.Identity;

namespace Persistance.Repos;

public class UserOrganizationRepo : GenericRepoAsync<UserOrganization>, IUserOrganizationRepo
{
    #region Fields
    private DbSet<UserOrganization> userOrganizations;
    #endregion

    #region Constructors
    public UserOrganizationRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        userOrganizations = dbContext.Set<UserOrganization>();
    }
    #endregion

    #region Handle Methods

    #endregion
}