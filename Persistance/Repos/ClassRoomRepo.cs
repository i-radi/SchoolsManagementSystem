namespace Persistance.Repos;

public class ClassroomRepo : GenericRepoAsync<Classroom>, IClassroomRepo
{
    #region Fields
    private DbSet<Classroom> classrooms;
    #endregion

    #region Constructors
    public ClassroomRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        classrooms = dbContext.Set<Classroom>();
    }
    #endregion

    #region Handle Methods
    public override async Task<Classroom> GetByIdAsync(int id)
    {
#pragma warning disable CS8603
        return await _dbContext.Set<Classroom>()
            .Include(c => c.Grade)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    #endregion
}
