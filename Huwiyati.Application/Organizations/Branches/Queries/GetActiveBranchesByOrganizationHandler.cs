namespace Huwiyati.Application.Organizations.Branches.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Branches.DTOs;

/// <summary>
/// Handles querying active service centers/branches for a specified active organization.
/// </summary>
public class GetActiveBranchesByOrganizationHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetActiveBranchesByOrganizationHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetActiveBranchesByOrganizationHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all active service delivery centers/branches belonging to an active organization.
    /// </summary>
    /// <param name="organizationId">The unique identifier of the organization.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a list of active <see cref="BranchDto"/> items.</returns>
    public async Task<ApiResponse<List<BranchDto>>> HandleAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        if (organizationId == Guid.Empty)
        {
            return ApiResponse<List<BranchDto>>.Failure("Organization identifier is required.", statusCode: 400);
        }

        var branches = await _context.OrganizationBranches
            .AsNoTracking()
            .Where(b => b.OrganizationId == organizationId && b.IsActive && b.Organization.IsActive)
            .OrderBy(b => b.Governorate)
            .ThenBy(b => b.BranchName)
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

        return ApiResponse<List<BranchDto>>.Success(branches, message: "Active service centers retrieved successfully.");
    }
}
