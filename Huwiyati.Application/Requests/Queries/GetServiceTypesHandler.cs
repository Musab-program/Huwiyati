using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Requests.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Huwiyati.Application.Requests.Queries;

/// <summary>
/// Handles querying active service types catalog for dropdown menus and lookup listings.
/// </summary>
public class GetServiceTypesHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetServiceTypesHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetServiceTypesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves active service types, optionally filtered by organization identifier.
    /// </summary>
    /// <param name="query">The service types query payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a list of <see cref="ServiceTypeDto"/> items.</returns>
    public async Task<ApiResponse<List<ServiceTypeDto>>> GetServiceTypesAsync(
        GetServiceTypesQuery query,
        CancellationToken cancellationToken = default)
    {
        var baseQuery = _context.ServiceTypes
            .AsNoTracking()
            .Where(st => st.IsActive);

        if (query.OrganizationId.HasValue && query.OrganizationId.Value != Guid.Empty)
        {
            baseQuery = baseQuery.Where(st => st.OrganizationId == query.OrganizationId.Value);
        }

        var serviceTypes = await baseQuery
            .OrderBy(st => st.Name)
            .Select(st => new ServiceTypeDto
            {
                Id = st.Id,
                Name = st.Name,
                Code = st.Code,
                OrganizationId = st.OrganizationId,
                OrganizationName = st.Organization.Name,
                IsActive = st.IsActive
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<ServiceTypeDto>>.Success(serviceTypes, message: "Active service types retrieved successfully.", statusCode: 200);
    }
}
