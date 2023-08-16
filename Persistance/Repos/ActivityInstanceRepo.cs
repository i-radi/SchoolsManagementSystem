namespace Persistance.Repos;

public class ActivityInstanceRepo : GenericRepoAsync<ActivityInstance>, IActivityInstanceRepo
{
    #region Fields
    private DbSet<ActivityInstance> _activities;
    #endregion

    #region Constructors
    public ActivityInstanceRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        _activities = dbContext.Set<ActivityInstance>();
    }
    #endregion

    #region Handle Methods
    #endregion

}
