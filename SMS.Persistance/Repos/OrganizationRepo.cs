﻿using Microsoft.EntityFrameworkCore;
using SMS.Models.Entities;
using SMS.Persistance.Context;
using SMS.Persistance.IRepos;

namespace SMS.Persistance.Repos;

public class OrganizationRepo : GenericRepoAsync<Organization>, IOrganizationRepo
{
    #region Fields
    private DbSet<Organization> organizations;
    #endregion

    #region Constructors
    public OrganizationRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        organizations = dbContext.Set<Organization>();
    }
    #endregion

    #region Handle Methods

    #endregion
}
