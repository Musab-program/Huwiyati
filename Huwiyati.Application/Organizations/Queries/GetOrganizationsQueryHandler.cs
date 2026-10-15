namespace Huwiyati.Application.Organizations.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.DTOs;

/// <summary>
/// Handles querying all active organizations for lookup listings.
/// </summary>
public class GetOrganizationsQueryHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetOrganizationsQueryHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetOrganizationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a list of all active organizations.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a list of <see cref="OrganizationLookupDto"/> items.</returns>
    public async Task<ApiResponse<List<OrganizationLookupDto>>> GetOrganizationsAsync(CancellationToken cancellationToken = default)
    {
        var organizations = await _context.Organizations
            .Where(o => o.IsActive)
            .Select(o => new OrganizationLookupDto
            {
                Id = o.Id,
                Name = o.Name
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<OrganizationLookupDto>>.Success(organizations);
    }
}