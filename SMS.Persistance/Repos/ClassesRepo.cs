namespace SMS.Persistance.Repos;

public class ClassesRepo : GenericRepoAsync<Classes>, IClassesRepo
{
    #region Fields
    private DbSet<Classes> classes;
    #endregion

    #region Constructors
    public ClassesRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        classes = dbContext.Set<Classes>();
    }
    #endregion

    #region Handle Methods
    public override async Task<Classes> GetByIdAsync(int id)
    {
#pragma warning disable CS8603
        return await _dbContext.Set<Classes>()
            .Include(c => c.Grade)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    #endregion
}
