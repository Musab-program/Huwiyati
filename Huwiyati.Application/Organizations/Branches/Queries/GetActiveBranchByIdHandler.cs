namespace Huwiyati.Application.Organizations.Branches.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Branches.DTOs;

/// <summary>
/// Handles querying details of a specific active service center/branch by its identifier.
/// </summary>
public class GetActiveBranchByIdHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetActiveBranchByIdHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetActiveBranchByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a single active service center/branch by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the branch.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the <see cref="BranchDto"/> if active and found; otherwise 404.</returns>
    public async Task<ApiResponse<BranchDto>> HandleAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            return ApiResponse<BranchDto>.Failure("Branch identifier is required.", statusCode: 400);
        }

        var branchDto = await _context.OrganizationBranches
            .AsNoTracking()
            .Where(b => b.Id == id && b.IsActive && b.Organization.IsActive)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (branchDto == null)
        {
            return ApiResponse<BranchDto>.Failure(
                "Active service center or branch was not found.",
                statusCode: 404);
        }

        return ApiResponse<BranchDto>.Success(branchDto, message: "Active service center details retrieved successfully.");
    }
}
