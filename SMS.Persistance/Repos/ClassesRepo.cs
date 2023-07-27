﻿namespace SMS.Persistance.Repos;

public class ClassesRepo : GenericRepoAsync<Classes>, IClassesRepo
{
    #region Fields
    private DbSet<Classes> classes;
    #endregion

    #region Constructors
    public ClassesRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        classes = dbContext.Set<Classes>();
    }
    #endregion

    #region Handle Methods

    #endregion
}
