namespace Persistance.Repos;

public class SchoolRepo : GenericRepoAsync<School>, ISchoolRepo
{
    #region Fields
    private DbSet<School> schools;
    #endregion

    #region Constructors
    public SchoolRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        schools = dbContext.Set<School>();
    }
    #endregion

    #region Handle Methods
    public override async Task<School> GetByIdAsync(int id)
    {
#pragma warning disable CS8603
        return await _dbContext.Set<School>()
            .Include(c => c.Organization)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    #endregion
}
