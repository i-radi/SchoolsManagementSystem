namespace Persistance.Repos;

public class ActivityTimeRepo : GenericRepoAsync<ActivityTime>, IActivityTimeRepo
{
    #region Fields
    private DbSet<ActivityTime> _activities;
    #endregion

    #region Constructors
    public ActivityTimeRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        _activities = dbContext.Set<ActivityTime>();
    }
    #endregion

    #region Handle Methods
    #endregion

}
