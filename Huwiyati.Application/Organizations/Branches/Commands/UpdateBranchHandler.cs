namespace Huwiyati.Application.Organizations.Branches.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Branches.DTOs;

/// <summary>
/// Handles updating configuration and contact details for an existing organization branch.
/// </summary>
public class UpdateBranchHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateBranchHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public UpdateBranchHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Updates the details of an organization branch asynchronously.
    /// </summary>
    /// <param name="command">The update branch command payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the updated <see cref="BranchDto"/>.</returns>
    public async Task<ApiResponse<BranchDto>> UpdateBranchAsync(
        UpdateBranchCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Fetch branch without Include
        var branch = await _context.OrganizationBranches
            .FirstOrDefaultAsync(b => b.Id == command.Id, cancellationToken);

        if (branch == null)
        {
            return ApiResponse<BranchDto>.Failure(
                "Organization branch was not found.",
                statusCode: 404);
        }

        // 2. Verify that the specified OrganizationId exists in database
        var organization = await _context.Organizations
            .FirstOrDefaultAsync(o => o.Id == command.OrganizationId, cancellationToken);

        if (organization == null)
        {
            return ApiResponse<BranchDto>.Failure(
                "The specified organization was not found.",
                statusCode: 404);
        }

        // 3. Update properties
        branch.OrganizationId = command.OrganizationId;
        branch.BranchName = command.BranchName;
        branch.Governorate = command.Governorate;
        branch.District = command.District;
        branch.AddressDetails = command.AddressDetails;
        branch.PhoneNumber = command.PhoneNumber;
        branch.LastModifiedAt = DateTime.UtcNow;

        _context.OrganizationBranches.Update(branch);
        await _context.SaveChangesAsync(cancellationToken);

        // 4. Map updated entity to BranchDto
        var resultDto = new BranchDto
        {
            Id = branch.Id,
            OrganizationId = branch.OrganizationId,
            OrganizationName = organization.Name,
            BranchName = branch.BranchName,
            Governorate = branch.Governorate,
            District = branch.District,
            AddressDetails = branch.AddressDetails,
            PhoneNumber = branch.PhoneNumber,
            IsActive = branch.IsActive,
            CreatedAt = branch.CreatedAt
        };

        return ApiResponse<BranchDto>.Success(
            resultDto,
            message: "Organization branch updated successfully.");
    }
}