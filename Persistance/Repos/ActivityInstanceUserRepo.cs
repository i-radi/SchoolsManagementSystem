
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
    public async Task<List<int>> GetUserIdsByActivityInstanceId(int acivityInstanceId)
    {

        var userIds = await _activities.AsNoTracking().Where(x => x.ActivityInstanceId == acivityInstanceId)
          .Select(x => x.UserId)
          .ToListAsync();

        return userIds;
    }
    #endregion

}