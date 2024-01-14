namespace Persistance.Repos;

public class CourseDetailsRepo : GenericRepoAsync<CourseDetails>, ICourseDetailsRepo
{
    #region Fields
    private DbSet<CourseDetails> _courseDetails;
    #endregion

    #region Constructors
    public CourseDetailsRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        _courseDetails = dbContext.Set<CourseDetails>();
    }
    #endregion

    #region Handle Methods
    #endregion

}
