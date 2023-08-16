namespace Persistance.Repos;

public class ActivityRepo : GenericRepoAsync<Activity>, IActivityRepo
{
    #region Fields
    private DbSet<Activity> _activities;
    #endregion

    #region Constructors
    public ActivityRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        _activities = dbContext.Set<Activity>();
    }
    #endregion

    #region Handle Methods
    public override async Task<Activity> GetByIdAsync(int id)
    {
#pragma warning disable CS8603
        return await _dbContext.Set<Activity>()
            .Include(c => c.School)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    #endregion
}
