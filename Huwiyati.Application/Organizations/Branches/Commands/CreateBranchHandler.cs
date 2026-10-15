using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Branches.DTOs;
using Huwiyati.Domain.Entities.Organizations;
using Microsoft.EntityFrameworkCore;

namespace Huwiyati.Application.Organizations.Branches.Commands;

/// <summary>
/// Handles creating a new branch office for an organization.
/// </summary>
public class CreateBranchHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateBranchHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public CreateBranchHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates an organization branch asynchronously.
    /// </summary>
    /// <param name="command">The create branch command payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the resulting <see cref="BranchDto"/>.</returns>
    public async Task<ApiResponse<BranchDto>> CreateBranchAsync(
        CreateBranchCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify that the organization exists and is active
        var organization = await _context.Organizations
            .FirstOrDefaultAsync(o => o.Id == command.OrganizationId && o.IsActive, cancellationToken);
        if (organization == null)
        {
            return ApiResponse<BranchDto>.Failure(
                "The specified organization was not found or is inactive.",
                statusCode: 404);
        }

        // 2. Create the new organization branch (IsActive defaults to true)
        var branch = new OrganizationBranch
        {
            OrganizationId = command.OrganizationId,
            BranchName = command.BranchName,
            Governorate = command.Governorate,
            District = command.District,
            AddressDetails = command.AddressDetails,
            PhoneNumber = command.PhoneNumber,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.OrganizationBranches.AddAsync(branch);
        await _context.SaveChangesAsync();

        // 3. Map created entity to BranchDto
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
           message: "Organization branch created successfully.");
    }
}
