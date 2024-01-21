
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


    public async Task<ActivityInstance> GetActivityInstanceById(int id)
    {
        var model = await _dbContext.ActivityInstances.Include(x => x.Activity).FirstOrDefaultAsync(x => x.Id == id);
        if (model == null) return null;
        return model;
    }
    #endregion

}
