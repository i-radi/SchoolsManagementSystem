namespace SMS.Persistance.Repos;

public class UserTypeRepo : GenericRepoAsync<UserType>, IUserTypeRepo
{
    #region Fields
    private DbSet<UserType> userTypes;
    #endregion

    #region Constructors
    public UserTypeRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        userTypes = dbContext.Set<UserType>();
    }
    #endregion

    #region Handle Methods

    #endregion
}