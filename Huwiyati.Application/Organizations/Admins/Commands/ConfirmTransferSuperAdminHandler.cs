namespace Huwiyati.Application.Organizations.Admins.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Constants;

/// <summary>
/// Handles confirming and executing the transfer of SuperAdmin privileges to a new citizen.
/// </summary>
public class ConfirmTransferSuperAdminHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmTransferSuperAdminHandler"/> class.
    /// </summary>
    public ConfirmTransferSuperAdminHandler(
        IApplicationDbContext context,
        IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Validates the OTP verification code and executes role transfer asynchronously.
    /// </summary>
    public async Task<ApiResponse<bool>> ConfirmTransferAsync(
        Guid currentSuperAdminUserId,
        ConfirmTransferSuperAdminCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify caller has SuperAdmin role
        var callerRoles = await _identityService.GetUserRolesAsync(currentSuperAdminUserId, cancellationToken);
        if (!callerRoles.Contains(AppRoles.SuperAdmin))
        {
            return ApiResponse<bool>.Failure(
                "Only an active SuperAdmin can confirm role transfer.",
                statusCode: 403);
        }

        // 2. Validate OTP code from database
        var now = DateTime.UtcNow;
        var validCode = await _context.VerificationCodes
            .Where(vc => vc.UserId == currentSuperAdminUserId && !vc.IsUsed && vc.ExpirationTime > now)
            .OrderByDescending(vc => vc.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (validCode == null)
        {
            return ApiResponse<bool>.Failure(
                "Verification code not found or expired. Please request a new code.",
                statusCode: 400);
        }

        if (validCode.Attempts >= 5)
        {
            validCode.IsUsed = true;
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResponse<bool>.Failure(
                "Maximum verification attempts exceeded. Code has been invalidated. Please request a new code.",
                statusCode: 400);
        }

        if (validCode.Code != command.OtpCode)
        {
            validCode.Attempts++;
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResponse<bool>.Failure(
                $"Invalid verification code. Remaining attempts: {5 - validCode.Attempts}.",
                statusCode: 400);
        }

        // 3. Mark code as consumed
        validCode.IsUsed = true;
        await _context.SaveChangesAsync(cancellationToken);

        // 4. Verify target citizen exists in Civil Registry
        var person = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.TargetNationalNumber, cancellationToken);

        if (person == null)
        {
            return ApiResponse<bool>.Failure(
                "Target citizen was not found in Civil Registry.",
                statusCode: 404);
        }

        if (person.PersonStatus == Huwiyati.Domain.Enums.PersonStatus.Deceased)
        {
            return ApiResponse<bool>.Failure(
                "Cannot transfer SuperAdmin role to a deceased citizen.",
                statusCode: 400);
        }

        // 5. Verify target citizen has an active user account
        var targetUserId = await _identityService.GetUserIdByNationalNumberAsync(command.TargetNationalNumber, cancellationToken);

        if (!targetUserId.HasValue || !await _identityService.IsUserActiveAsync(targetUserId.Value, cancellationToken))
        {
            return ApiResponse<bool>.Failure(
                "Target citizen does not have an active user account in the system.",
                statusCode: 400);
        }

        if (targetUserId.Value == currentSuperAdminUserId)
        {
            return ApiResponse<bool>.Failure(
                "Target user cannot be the current SuperAdmin.",
                statusCode: 400);
        }

        // 6. Verify target user is NOT an active employee or admin in any branch
        bool isBranchStaff = await _context.Employees
            .AnyAsync(e => e.UserId == targetUserId.Value && e.IsActive, cancellationToken);

        if (isBranchStaff)
        {
            return ApiResponse<bool>.Failure(
                "Target citizen is currently assigned as an active employee or administrator in an organization branch. Please relieve them first.",
                statusCode: 400);
        }

        // 7. Execute transfer (Zero Lockout: assign new role first, then revoke from caller)
        await _identityService.AssignUserRolesAsync(targetUserId.Value, new[] { AppRoles.SuperAdmin }, cancellationToken);
        await _identityService.RemoveUserRoleAsync(currentSuperAdminUserId, AppRoles.SuperAdmin, cancellationToken);

        return ApiResponse<bool>.Success(
            true,
            message: "SuperAdmin privileges transferred successfully. You have been relieved of SuperAdmin duties.");
    }
}
