namespace Persistance.Repos;

public class ClassRoomRepo : GenericRepoAsync<ClassRoom>, IClassRoomRepo
{
    #region Fields
    private DbSet<ClassRoom> classRooms;
    #endregion

    #region Constructors
    public ClassRoomRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        classRooms = dbContext.Set<ClassRoom>();
    }
    #endregion

    #region Handle Methods
    public override async Task<ClassRoom> GetByIdAsync(int id)
    {
#pragma warning disable CS8603
        return await _dbContext.Set<ClassRoom>()
            .Include(c => c.Grade)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    #endregion
}
