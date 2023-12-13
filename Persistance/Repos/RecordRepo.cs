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

    public async Task<Record?> GetRecordWithUsersByRecordId(int id)
    {
        return await _dbContext.Set<Record>()
            .Include(c => c.UserRecords)
            .ThenInclude(ur => ur.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<UserClass>?> GetUserClassesByRecordId(int id)
    {
        return await _dbContext.Set<UserClass>()
                .Include(a => a.UserType)
                .Include(c => c.Classroom)
                .Include(b => b.User)
                .ThenInclude(ac => ac.UserRecords.Where(ac => ac.RecordId == id))
                .Where(c => c.User.UserRecords.Any(ac => ac.RecordId == id)
                && c.Season.IsCurrent)
                .AsSplitQuery()
                .ToListAsync();
    }
    #endregion
}
