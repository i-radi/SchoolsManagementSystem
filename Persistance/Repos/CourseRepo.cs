namespace Persistance.Repos;

public class CourseRepo : GenericRepoAsync<Course>, ICourseRepo
{
    #region Fields
    private DbSet<Course> courses;
    #endregion

    #region Constructors
    public CourseRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        courses = dbContext.Set<Course>();
    }
    #endregion

    #region Handle Methods
    public override async Task<Course> GetByIdAsync(int id)
    {
#pragma warning disable CS8603
        return await _dbContext.Set<Course>()
            .Include(c => c.School)
            .Include(c => c.CourseDetails)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    #endregion
}
