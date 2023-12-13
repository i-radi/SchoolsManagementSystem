using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace VModels.Utilities;

public static class PaginatedListExtentions
{
    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageIndex = 1, int pageSize = 10)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }

    public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> source, int pageIndex = 1, int pageSize = 10)
    {
        var count = source.Count();
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }

    public static PaginatedList<T> ToPaginatedList<T>(this IEnumerable<T> source, int pageIndex = 1, int pageSize = 10)
    {
        var enumerable = source.ToList();
        var count = enumerable.Count;
        var items = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }

    public static void AddPaginationHeader<T>(this HttpResponse response, PaginatedList<T> list)
    {
        var paginationHeader = new PaginationHeader<T>(list);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}

public class PaginatedList<T> : List<T>
{
    public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex = 1, int pageSize = 10)
    {
        var enumerable = source.ToList();
        var count = enumerable.Count;
        var items = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}

public class PaginationHeader<T>
{
    public PaginationHeader(PaginatedList<T> list)
    {
        PageIndex = list.PageIndex;
        TotalPages = list.TotalPages;
        PageSize = list.PageSize;
        TotalCount = list.TotalCount;
        HasPreviousPage = list.HasPreviousPage;
        HasNextPage = list.HasNextPage;
    }

    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }
    public bool HasPreviousPage { get; private set; }
    public bool HasNextPage { get; private set; }
}