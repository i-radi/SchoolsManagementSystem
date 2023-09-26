

using Microsoft.EntityFrameworkCore;
using Models.Entities.Identity;
using Tests.Wrappers.Interfaces;

namespace Tests.Wrappers.Implementations
{
    public class PaginatedService : IPaginatedService<User>
    {
        public async Task<PaginatedList<User>> ReturnPaginatedResult(IQueryable<User> source, int pageNumber, int pageSize)
        {
            return PaginatedList<User>.Create(await source.ToListAsync(),pageNumber, pageSize);
        }
    }
}
