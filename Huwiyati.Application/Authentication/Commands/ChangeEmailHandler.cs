namespace Huwiyati.Application.Authentication.Commands;

using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Authentication;

/// <summary>
/// Handles requesting an email address change by validating current credentials and dispatching an OTP to the new email.
/// </summary>
public class ChangeEmailHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeEmailHandler"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="identityService">The identity management service.</param>
    /// <param name="emailService">The email dispatching service.</param>
    public ChangeEmailHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IEmailService emailService)
    {
        _context = context;
        _identityService = identityService;
        _emailService = emailService;
    }

    /// <summary>
    /// Initiates email change by validating password, ensuring email uniqueness, and dispatching an OTP.
    /// </summary>
    /// <param name="userClaims">Current authenticated user claims.</param>
    /// <param name="command">Command containing current password and the new email.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with confirmation message.</returns>
    public async Task<ApiResponse<string>> HandleAsync(
        ClaimsPrincipal userClaims,
        ChangeEmailCommand command,
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

        // 2. Ensure new email is not already taken
        var emailExists = await _identityService.UserExistsWithEmailAsync(command.NewEmail, cancellationToken);
        if (emailExists)
        {
            return ApiResponse<string>.Failure("This email address is already in use by another account.", statusCode: 400);
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

        // 5. Send OTP to the NEW email
        var emailSent = await _emailService.SendOtpEmailAsync(
            command.NewEmail,
            "رمز تأكيد تغيير البريد الإلكتروني - هويتي",
            otpCode,
            "تغيير البريد الإلكتروني",
            cancellationToken);

        if (!emailSent)
        {
            return ApiResponse<string>.Failure(
                "Failed to send verification code to the new email address. Please verify email settings.",
                statusCode: 500);
        }

        return ApiResponse<string>.Success(
            "OTP Sent",
            message: $"A verification code has been sent to '{command.NewEmail}'. Please enter the code to confirm changing your email.");
    }
}
