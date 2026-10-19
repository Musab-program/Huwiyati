namespace Huwiyati.Application.Authentication.Commands;

using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Handles confirming phone number changes by validating OTP and applying the new phone number to the account.
/// </summary>
public class ConfirmChangePhoneHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmChangePhoneHandler"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="identityService">The identity management service.</param>
    public ConfirmChangePhoneHandler(
        IApplicationDbContext context,
        IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Confirms the phone number change using the OTP code and new phone number submitted in the command.
    /// </summary>
    /// <param name="userClaims">Current authenticated user claims.</param>
    /// <param name="command">Command containing the new phone number and 6-digit OTP code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating whether the phone number was successfully updated.</returns>
    public async Task<ApiResponse<bool>> HandleAsync(
        ClaimsPrincipal userClaims,
        ConfirmChangePhoneCommand command,
        CancellationToken cancellationToken = default)
    {
        var userId = _identityService.GetUserIdFromClaims(userClaims);
        if (userId == Guid.Empty)
        {
            return ApiResponse<bool>.Failure("Unauthorized user.", statusCode: 401);
        }

        // 1. Validate active verification code
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

        // 2. Mark verification code as used
        verificationCode.IsUsed = true;
        await _context.SaveChangesAsync(cancellationToken);

        // 3. Update phone number in Identity
        var updated = await _identityService.ChangePhoneNumberAsync(userId, command.NewPhoneNumber, cancellationToken);
        if (!updated)
        {
            return ApiResponse<bool>.Failure("Failed to update phone number.", statusCode: 500);
        }

        return ApiResponse<bool>.Success(true, message: "Phone number has been updated successfully.");
    }
}
