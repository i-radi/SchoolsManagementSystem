namespace SMS.Persistance.Repos;

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

    #endregion
}
