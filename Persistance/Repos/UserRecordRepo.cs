namespace Persistance.Repos;

public class UserRecordRepo : GenericRepoAsync<UserRecord>, IUserRecordRepo
{
    #region Fields
    private DbSet<UserRecord> _userRecords;
    #endregion

    #region Constructors
    public UserRecordRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        _userRecords = dbContext.Set<UserRecord>();
    }
    #endregion

    #region Handle Methods
    #endregion

}
