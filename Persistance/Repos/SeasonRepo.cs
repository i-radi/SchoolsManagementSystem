namespace Persistance.Repos;

public class SeasonRepo : GenericRepoAsync<Season>, ISeasonRepo
{
    #region Fields
    private DbSet<Season> seasons;
    #endregion

    #region Constructors
    public SeasonRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        seasons = dbContext.Set<Season>();
    }
    #endregion

    #region Handle Methods
    public override async Task<Season> GetByIdAsync(int id)
    {
#pragma warning disable CS8603
        return await _dbContext.Set<Season>()
            .Include(c => c.School)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    #endregion
}
