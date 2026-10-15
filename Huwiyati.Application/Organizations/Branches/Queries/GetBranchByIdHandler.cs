namespace Huwiyati.Application.Organizations.Branches.Queries;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Branches.DTOs;

/// <summary>
/// Handles querying details of a specific organization branch by its unique identifier.
/// </summary>
public class GetBranchByIdHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetBranchByIdHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetBranchByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a branch by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The branch identifier.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the resulting <see cref="BranchDto"/>.</returns>
    public async Task<ApiResponse<BranchDto>> GetBranchByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var branchDto = await _context.OrganizationBranches
            .AsNoTracking()
            .Where(b => b.Id == id)
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
                "Organization branch was not found.",
                statusCode: 404);
        }

        return ApiResponse<BranchDto>.Success(branchDto);
    }
}
