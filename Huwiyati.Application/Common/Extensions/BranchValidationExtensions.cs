namespace Huwiyati.Application.Common.Extensions;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Common.Models;

/// <summary>
/// Provides extension methods for validating organization branches and their operational status against business requirements.
/// </summary>
public static class BranchValidationExtensions
{
    /// <summary>
    /// Validates whether a given organization branch exists and whether both it and its parent organization are active.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="branchId">The unique identifier of the branch to validate.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="BranchValidationModel"/> describing the validation result and branch metadata.</returns>
    public static async Task<BranchValidationModel> ValidateActiveBranchAsync(
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
                ErrorMessage = "Specified branch does not exist.",
                StatusCode = 404
            };
        }

        if (!branchData.IsActive || !branchData.OrganizationIsActive)
        {
            return new BranchValidationModel
            {
                IsValid = false,
                ErrorMessage = "Specified branch or its parent organization is inactive.",
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

    /// <summary>
    /// Validates whether a given organization branch belongs specifically to the Civil Registry authority and is active.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="branchId">The unique identifier of the branch to validate.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="BranchValidationModel"/> describing the validation result and branch metadata.</returns>
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

        if (!branchData.OrganizationName.Contains("الأحوال المدنية"))
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

    /// <summary>
    /// Validates whether a given organization branch belongs specifically to the Immigration and Passport Authority and is active.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="branchId">The unique identifier of the branch to validate.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="BranchValidationModel"/> describing the validation result and branch metadata.</returns>
    public static async Task<BranchValidationModel> ValidateImmigrationBranchAsync(
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

        if (!branchData.OrganizationName.Contains("الجوازات") &&
            !branchData.OrganizationName.Contains("الهجرة") &&
            !branchData.OrganizationName.Contains("Passport") &&
            !branchData.OrganizationName.Contains("Immigration"))
        {
            return new BranchValidationModel
            {
                IsValid = false,
                ErrorMessage = "Passport operations can only be processed by Immigration and Passports branches.",
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
