namespace Huwiyati.Application.Common.Models;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents a paginated subset of a queryable collection of items.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
public class PaginatedList<T>
{
    /// <summary>
    /// Gets the items on the current page.
    /// </summary>
    public List<T> Items { get; }

    /// <summary>
    /// Gets the 1-based index of the current page.
    /// </summary>
    public int PageIndex { get; }

    /// <summary>
    /// Gets the total count of available pages based on page size.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Gets the total count of items across all pages.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedList{T}"/> class.
    /// </summary>
    /// <param name="items">The items on the current page.</param>
    /// <param name="count">Total count of items across all pages.</param>
    /// <param name="pageIndex">The current page index.</param>
    /// <param name="pageSize">The page capacity/size.</param>
    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    /// <summary>
    /// Gets a value indicating whether there is a preceding page before the current page.
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>
    /// Gets a value indicating whether there is a succeeding page after the current page.
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;

    /// <summary>
    /// Asynchronously creates a paginated list by executing a count and skip/take query against an <see cref="IQueryable{T}"/> source.
    /// </summary>
    /// <param name="source">The queryable data source.</param>
    /// <param name="pageIndex">The 1-based page index.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A populated <see cref="PaginatedList{T}"/> instance.</returns>
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
