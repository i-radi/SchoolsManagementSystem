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
        return await _dbContext.Set<Activity>()
            .Include(c => c.School)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<UserClass>?> GetUserClassesByActivityId(int id)
    {
        return await _dbContext.Set<UserClass>()
            .Include(a => a.UserType)
            .Include(b => b.User)
            .Include(c => c.Classroom)
            .ThenInclude(ac => ac.ActivityClassrooms.Where(ac => ac.ActivityId == id))
            .Where(c => c.Classroom.ActivityClassrooms.Any(ac => ac.ActivityId == id)
            && c.Season.IsCurrent)
            .ToListAsync();
    }

    public async Task<List<ActivityInstance>?> GetActivityInstancesWithUserByActivityId(int id)
    {
        return await _dbContext.Set<ActivityInstance>()
            .Include(c => c.ActivityInstanceUsers)
            .Where(c => c.Season.IsCurrent
            && c.ActivityId == id)
            .ToListAsync();
    }
    #endregion
}
