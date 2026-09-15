namespace Huwiyati.Application.Organizations.Branches.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Organizations.Branches.DTOs;

public class UpdateBranchHandler
{
    private readonly IApplicationDbContext _context;

    public UpdateBranchHandler(IApplicationDbContext context)
    {
        _context = context;
    }

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