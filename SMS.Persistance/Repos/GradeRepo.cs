using Microsoft.EntityFrameworkCore;
using SMS.Models.Entities;
using SMS.Persistance.Context;
using SMS.Persistance.IRepos;

namespace SMS.Persistance.Repos;

public class GradeRepo : GenericRepoAsync<Grade>, IGradeRepo
{
    #region Fields
    private DbSet<Grade> grades;
    #endregion

    #region Constructors
    public GradeRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        grades = dbContext.Set<Grade>();
    }
    #endregion

    #region Handle Methods

    #endregion
}
