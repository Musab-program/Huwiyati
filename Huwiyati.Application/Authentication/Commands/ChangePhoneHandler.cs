namespace Huwiyati.Application.Authentication.Commands;

using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Authentication;

/// <summary>
/// Handles requesting a phone number change by validating credentials and dispatching an OTP to the user's registered email.
/// </summary>
public class ChangePhoneHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePhoneHandler"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="identityService">The identity management service.</param>
    /// <param name="emailService">The email dispatching service.</param>
    public ChangePhoneHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IEmailService emailService)
    {
        _context = context;
        _identityService = identityService;
        _emailService = emailService;
    }

    /// <summary>
    /// Validates password, generates an OTP, and sends it to the user's registered email to authorize changing the phone number.
    /// </summary>
    /// <param name="userClaims">Current authenticated user claims.</param>
    /// <param name="command">Command containing current password and new phone number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating OTP dispatch status.</returns>
    public async Task<ApiResponse<string>> HandleAsync(
        ClaimsPrincipal userClaims,
        ChangePhoneCommand command,
        CancellationToken cancellationToken = default)
    {
        var userId = _identityService.GetUserIdFromClaims(userClaims);
        if (userId == Guid.Empty)
        {
            return ApiResponse<string>.Failure("Unauthorized user.", statusCode: 401);
        }

        // 1. Verify current password
        var isPasswordValid = await _identityService.VerifyUserPasswordAsync(userId, command.CurrentPassword, cancellationToken);
        if (!isPasswordValid)
        {
            return ApiResponse<string>.Failure("Current password is incorrect.", statusCode: 400);
        }

        // 2. Fetch user's registered email to send OTP
        var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(userId, cancellationToken);
        if (!contactInfo.HasValue || string.IsNullOrWhiteSpace(contactInfo.Value.Email))
        {
            return ApiResponse<string>.Failure("No registered email address found for this account.", statusCode: 400);
        }

        // 3. Invalidate previous active verification codes for this user
        var previousCodes = await _context.VerificationCodes
            .Where(vc => vc.UserId == userId && !vc.IsUsed)
            .ToListAsync(cancellationToken);

        foreach (var c in previousCodes)
        {
            c.IsUsed = true;
        }

        // 4. Generate 6-digit OTP valid for 5 minutes
        var otpCode = new Random().Next(100000, 999999).ToString();
        var verificationCode = new VerificationCode
        {
            UserId = userId,
            Code = otpCode,
            ExpirationTime = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.VerificationCodes.AddAsync(verificationCode, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 5. Send OTP to registered email
        var emailSent = await _emailService.SendOtpEmailAsync(
            contactInfo.Value.Email,
            "رمز تأكيد تغيير رقم الهاتف - هويتي",
            otpCode,
            "تغيير رقم الهاتف",
            cancellationToken);

        if (!emailSent)
        {
            return ApiResponse<string>.Failure(
                "Failed to send verification code to your email. Please try again later.",
                statusCode: 500);
        }

        return ApiResponse<string>.Success(
            "OTP Sent",
            message: $"A verification code has been sent to your registered email ({contactInfo.Value.Email}). Please confirm the code to complete the phone number update.");
    }
}
