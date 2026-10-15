namespace Huwiyati.Application.Authentication.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Domain.Events.Authentication;

/// <summary>
/// Handles forgot password requests by generating an OTP verification code, dispatching an email to the user, and triggering domain events.
/// </summary>
public class ForgotPasswordHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;
    private readonly IDomainEventHandler<PasswordResetRequestedEvent> _passwordResetRequestedEventHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ForgotPasswordHandler"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    /// <param name="identityService">The identity management service.</param>
    /// <param name="emailService">The email dispatching service.</param>
    /// <param name="passwordResetRequestedEventHandler">The domain event handler for password reset notifications.</param>
    public ForgotPasswordHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IEmailService emailService,
        IDomainEventHandler<PasswordResetRequestedEvent> passwordResetRequestedEventHandler)
    {
        _context = context;
        _identityService = identityService;
        _emailService = emailService;
        _passwordResetRequestedEventHandler = passwordResetRequestedEventHandler;
    }

    /// <summary>
    /// Processes a forgot password request asynchronously.
    /// </summary>
    /// <param name="command">The forgot password command details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating whether the reset OTP was successfully triggered.</returns>
    public async Task<ApiResponse<bool>> RequestPasswordResetAsync(
        ForgotPasswordCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Find UserId by National Number
        var userId = await _identityService.GetUserIdByNationalNumberAsync(command.NationalNumber, cancellationToken);

        // 2. If user exists, generate OTP code valid for 5 minutes, save to Database, and send via Email
        if (userId.HasValue && userId.Value != Guid.Empty)
        {
            var otpCode = new Random().Next(100000, 999999).ToString();
            var verificationCode = new VerificationCode
            {
                UserId = userId.Value,
                Code = otpCode,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _context.VerificationCodes.AddAsync(verificationCode, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // Trigger PasswordResetRequestedEvent
            var passwordResetRequestedEvent = new PasswordResetRequestedEvent(userId.Value);
            await _passwordResetRequestedEventHandler.HandleAsync(passwordResetRequestedEvent, cancellationToken);

            // Fetch user email via IdentityService and send OTP email securely
            var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(userId.Value, cancellationToken);
            if (contactInfo.HasValue && !string.IsNullOrWhiteSpace(contactInfo.Value.Email))
            {
                var emailSent = await _emailService.SendOtpEmailAsync(
                    contactInfo.Value.Email,
                    "رمز التحقق - استعادة كلمة المرور",
                    otpCode,
                    "إعادة تعيين كلمة المرور",
                    cancellationToken);

                if (!emailSent)
                {
                    return ApiResponse<bool>.Failure(
                        "Failed to send password reset OTP email. Please try again later.",
                        statusCode: 500);
                }
            }
        }

        // 3. Return generic security success response
        return ApiResponse<bool>.Success(
            true,
            message: "If an account with this National Number exists, a verification OTP code has been sent to your registered email.");
    }
}