namespace Persistance.Repos;

public class UserClassRepo : GenericRepoAsync<UserClass>, IUserClassRepo
{
    #region Fields
    private DbSet<UserClass> userClasses;
    #endregion

    #region Constructors
    public UserClassRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        userClasses = dbContext.Set<UserClass>();
    }
    #endregion

    #region Handle Methods
    public override async Task<UserClass> GetByIdAsync(int id)
    {
#pragma warning disable CS8603
        return await _dbContext.Set<UserClass>()
            .Include(c => c.User)
            .Include(c => c.Classes)
            .Include(c => c.UserType)
            .Include(c => c.Season)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    #endregion
}