namespace SMS.Persistance.Repos;

public class UserClassRepo : GenericRepoAsync<UserClass>, IUserClassRepo
{
    #region Fields
    private DbSet<UserClass> userClasses;
    #endregion

    #region Constructors
    public UserClassRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        userClasses = dbContext.Set<UserClass>();
    }
    #endregion

    #region Handle Methods

    #endregion
}