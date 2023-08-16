namespace Persistance.Repos;

public class ActivityInstanceSeasonRepo : GenericRepoAsync<ActivityInstanceSeason>, IActivityInstanceSeasonRepo
{
    #region Fields
    private DbSet<ActivityInstanceSeason> _activities;
    #endregion

    #region Constructors
    public ActivityInstanceSeasonRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        _activities = dbContext.Set<ActivityInstanceSeason>();
    }
    #endregion

    #region Handle Methods
    #endregion

}
