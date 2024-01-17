
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

    public async Task<ActivityInstance> AddActivityInstanceAsync(ActivityInstance activityInstance)
    {
   
        var model = await _dbContext.ActivityInstances.AddAsync(activityInstance);
        await _dbContext.SaveChangesAsync();
        return model.Entity;
    
    }
    #endregion

    #region Handle Methods
    #endregion

}
