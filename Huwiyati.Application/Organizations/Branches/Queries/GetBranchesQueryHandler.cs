namespace Huwiyati.Application.Organizations.Branches.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Branches.DTOs;

/// <summary>
/// Handles querying organization branches, optionally filtered by organization.
/// </summary>
public class GetBranchesQueryHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetBranchesQueryHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetBranchesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a list of branches optionally filtered by organization identifier.
    /// </summary>
    /// <param name="organizationId">Optional organization filter identifier.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a list of <see cref="BranchDto"/> items.</returns>
    public async Task<ApiResponse<List<BranchDto>>> GetBranchesAsync(
        Guid? organizationId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.OrganizationBranches.AsNoTracking();

        if (organizationId.HasValue)
        {
            query = query.Where(b => b.OrganizationId == organizationId.Value);
        }

        var branches = await query
            .Select(b => new BranchDto
            {
                Id = b.Id,
                OrganizationId = b.OrganizationId,
                OrganizationName = b.Organization.Name,
                BranchName = b.BranchName,
                Governorate = b.Governorate,
                District = b.District,
                AddressDetails = b.AddressDetails,
                PhoneNumber = b.PhoneNumber,
                IsActive = b.IsActive,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return ApiResponse<List<BranchDto>>.Success(branches);
    }
}