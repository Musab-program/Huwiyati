namespace Huwiyati.Application.Authentication.Commands;

using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Handles confirming email address changes by validating OTP and applying the new email address.
/// </summary>
public class ConfirmChangeEmailHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmChangeEmailHandler"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="identityService">The identity management service.</param>
    public ConfirmChangeEmailHandler(
        IApplicationDbContext context,
        IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Confirms the email change using the OTP code and new email submitted in the command.
    /// </summary>
    /// <param name="userClaims">Current authenticated user claims.</param>
    /// <param name="command">Command containing the new email and 6-digit OTP code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating whether the email was successfully updated.</returns>
    public async Task<ApiResponse<bool>> HandleAsync(
        ClaimsPrincipal userClaims,
        ConfirmChangeEmailCommand command,
        CancellationToken cancellationToken = default)
    {
        var userId = _identityService.GetUserIdFromClaims(userClaims);
        if (userId == Guid.Empty)
        {
            return ApiResponse<bool>.Failure("Unauthorized user.", statusCode: 401);
        }

        // 1. Ensure new email is still available
        var emailExists = await _identityService.UserExistsWithEmailAsync(command.NewEmail, cancellationToken);
        if (emailExists)
        {
            return ApiResponse<bool>.Failure("This email address is already in use by another account.", statusCode: 400);
        }

        // 2. Validate active verification code
        var verificationCode = await _context.VerificationCodes
            .Where(vc => vc.UserId == userId && !vc.IsUsed)
            .OrderByDescending(vc => vc.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (verificationCode == null || verificationCode.Code != command.Code)
        {
            return ApiResponse<bool>.Failure("Invalid verification code.", statusCode: 400);
        }

        if (verificationCode.ExpirationTime < DateTime.UtcNow)
        {
            return ApiResponse<bool>.Failure("Verification code has expired. Please request a new one.", statusCode: 400);
        }

        // 3. Mark verification code as used
        verificationCode.IsUsed = true;
        await _context.SaveChangesAsync(cancellationToken);

        // 4. Update email in Identity
        var updated = await _identityService.ChangeEmailAsync(userId, command.NewEmail, cancellationToken);
        if (!updated)
        {
            return ApiResponse<bool>.Failure("Failed to update email address.", statusCode: 500);
        }

        // 5. Create in-system notification for the user
        var notification = new Huwiyati.Domain.Entities.Notifications.Notification
        {
            UserId = userId,
            Title = "تحديث البريد الإلكتروني",
            Message = $"تم تحديث بريدك الإلكتروني المسجل في نظام هويتي بنجاح إلى ({command.NewEmail}).",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Success(true, message: "Email address has been updated successfully.");
    }
}
