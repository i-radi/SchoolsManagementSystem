using Models.Entities;

namespace Persistance.Repos;

public class ActivityInstanceUserRepo : GenericRepoAsync<ActivityInstanceUser>, IActivityInstanceUserRepo
{
    #region Fields
    private DbSet<ActivityInstanceUser> _activities;
    #endregion

    #region Constructors
    public ActivityInstanceUserRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        _activities = dbContext.Set<ActivityInstanceUser>();
    }
    #endregion

    #region Handle Methods
    #endregion

}
