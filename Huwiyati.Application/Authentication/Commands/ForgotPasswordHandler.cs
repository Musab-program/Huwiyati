namespace Huwiyati.Application.Authentication.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Authentication;

public class ForgotPasswordHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;

    public ForgotPasswordHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IEmailService emailService)
    {
        _context = context;
        _identityService = identityService;
        _emailService = emailService;
    }

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

            // Fetch user email via IdentityService and send OTP email securely
            var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(userId.Value, cancellationToken);
            if (contactInfo.HasValue && !string.IsNullOrWhiteSpace(contactInfo.Value.Email))
            {
                await _emailService.SendOtpEmailAsync(
                    contactInfo.Value.Email,
                    "رمز التحقق - استعادة كلمة المرور",
                    otpCode,
                    "إعادة تعيين كلمة المرور",
                    cancellationToken);
            }
        }

        // 3. Return generic security success response
        return ApiResponse<bool>.Success(
            true,
            message: "If an account with this National Number exists, a verification OTP code has been sent to your registered email.");
    }
}