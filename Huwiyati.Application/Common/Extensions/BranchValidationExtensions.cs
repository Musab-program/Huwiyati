namespace Huwiyati.Application.Common.Extensions;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Common.Models;

public static class BranchValidationExtensions
{
    public static async Task<BranchValidationModel> ValidateCivilRegistryBranchAsync(
        this IApplicationDbContext context,
        Guid branchId,
        CancellationToken cancellationToken = default)
    {
        var branchData = await context.OrganizationBranches
            .AsNoTracking()
            .Where(b => b.Id == branchId)
            .Select(b => new
            {
                b.Id,
                b.BranchName,
                b.IsActive,
                b.OrganizationId,
                OrganizationIsActive = b.Organization.IsActive,
                OrganizationName = b.Organization.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (branchData == null)
        {
            return new BranchValidationModel
            {
                IsValid = false,
                ErrorMessage = "Specified issuing branch does not exist.",
                StatusCode = 404
            };
        }

        if (!branchData.IsActive || !branchData.OrganizationIsActive)
        {
            return new BranchValidationModel
            {
                IsValid = false,
                ErrorMessage = "Specified issuing branch or its parent organization is inactive.",
                StatusCode = 400
            };
        }

        if (!branchData.OrganizationName.Contains("الأحوال المدنية") )
        {
            return new BranchValidationModel
            {
                IsValid = false,
                ErrorMessage = "Document operations can only be processed by Civil Registry branches.",
                StatusCode = 400
            };
        }

        return new BranchValidationModel
        {
            IsValid = true,
            StatusCode = 200,
            BranchId = branchData.Id,
            BranchName = branchData.BranchName,
            OrganizationId = branchData.OrganizationId,
            OrganizationName = branchData.OrganizationName
        };
    }
}
