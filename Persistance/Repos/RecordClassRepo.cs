namespace Persistance.Repos;

public class RecordClassRepo : GenericRepoAsync<RecordClass>, IRecordClassRepo
{
    #region Fields
    private DbSet<RecordClass> _recordClasses;
    #endregion

    #region Constructors
    public RecordClassRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        _recordClasses = dbContext.Set<RecordClass>();
    }
    #endregion

    #region Handle Methods
    #endregion

}
