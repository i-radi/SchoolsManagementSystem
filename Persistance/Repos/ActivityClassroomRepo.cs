namespace Persistance.Repos;

public class ActivityClassroomRepo : GenericRepoAsync<ActivityClassroom>, IActivityClassroomRepo
{
    #region Fields
    private DbSet<ActivityClassroom> _activities;
    #endregion

    #region Constructors
    public ActivityClassroomRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        _activities = dbContext.Set<ActivityClassroom>();
    }
    #endregion

    #region Handle Methods
    #endregion

}
