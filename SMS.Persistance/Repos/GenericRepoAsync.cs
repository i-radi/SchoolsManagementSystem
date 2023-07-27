﻿using Microsoft.EntityFrameworkCore.Storage;

namespace SMS.Persistance.Repos;

public class GenericRepoAsync<T> : IGenericRepoAsync<T> where T : class
{
    #region Vars / Props

    protected readonly ApplicationDBContext _dbContext;

    #endregion

    #region Constructor(s)
    public GenericRepoAsync(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    #region Methods

    #endregion

    #region Actions

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public IQueryable<T> GetTableNoTracking()
    {
        return _dbContext.Set<T>().AsNoTracking().AsQueryable();
    }

    public IQueryable<T> GetTableAsTracking()
    {
        return _dbContext.Set<T>().AsQueryable();
    }


    public virtual async Task<T> AddAsync(T entity)
    {
        var model = await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return model.Entity;
    }

    public virtual async Task AddRangeAsync(ICollection<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateRangeAsync(ICollection<T> entities)
    {
        _dbContext.Set<T>().UpdateRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteRangeAsync(ICollection<T> entities)
    {
        foreach (var entity in entities)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }
        await _dbContext.SaveChangesAsync();
    }

    public IDbContextTransaction BeginTransaction()
    {
        return _dbContext.Database.BeginTransaction();
    }

    public void Commit()
    {
        _dbContext.Database.CommitTransaction();
    }

    public void RollBack()
    {
        _dbContext.Database.RollbackTransaction();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    #endregion
}
