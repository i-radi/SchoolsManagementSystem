namespace SMS.Persistance.Repos;

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

    #endregion
}
