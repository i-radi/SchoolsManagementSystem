using Models.Helpers;

namespace Tests.Wrappers.Interfaces
{
    public interface IPaginatedService<T>
    {
        public Task<PaginatedList<T>> ReturnPaginatedResult(IQueryable<T> source, int pageNumber, int pageSize);
    }
}
