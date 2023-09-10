namespace Persistance.Repos;

public class RecordRepo : GenericRepoAsync<Record>, IRecordRepo
{
    #region Fields
    private DbSet<Record> records;
    #endregion

    #region Constructors
    public RecordRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        records = dbContext.Set<Record>();
    }
    #endregion

    #region Handle Methods
    public override async Task<Record> GetByIdAsync(int id)
    {
#pragma warning disable CS8603
        return await _dbContext.Set<Record>()
            .Include(c => c.School)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    #endregion
}
