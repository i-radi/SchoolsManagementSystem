namespace Persistance.Repos;

public class GradeRepo : GenericRepoAsync<Grade>, IGradeRepo
{
    #region Fields
    private DbSet<Grade> grades;
    #endregion

    #region Constructors
    public GradeRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        grades = dbContext.Set<Grade>();
    }
    #endregion

    #region Handle Methods
    public override async Task<Grade> GetByIdAsync(int id)
    {
#pragma warning disable CS8603
        return await _dbContext.Set<Grade>()
            .Include(c => c.School)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    #endregion
}
