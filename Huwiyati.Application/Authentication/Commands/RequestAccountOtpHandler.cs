namespace Huwiyati.Application.Authentication.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Authentication;

public class RequestAccountOtpHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;

    public RequestAccountOtpHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IEmailService emailService)
    {
        _context = context;
        _identityService = identityService;
        _emailService = emailService;
    }

    public async Task<ApiResponse<string>> RequestOtpAsync(
        RequestAccountOtpCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Validate Credentials
        var userLoginInfo = await _identityService.CheckUserExistAsync(command.NationalNumber, command.Password, cancellationToken);
        if (userLoginInfo == null || !userLoginInfo.Succeeded)
        {
            return ApiResponse<string>.Failure("Invalid National Number or Password.", statusCode: 401);
        }

        // 2. Generate OTP Code and Save to Database
        var otpCode = new Random().Next(100000, 999999).ToString();
        var verificationCode = new VerificationCode
        {
            UserId = userLoginInfo.UserId,
            Code = otpCode,
            ExpirationTime = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.VerificationCodes.AddAsync(verificationCode, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 3. Get User Contact Email and send OTP email securely
        var contactInfo = await _identityService.GetUserContactAndPersonIdAsync(userLoginInfo.UserId, cancellationToken);

        var hasEmail = contactInfo.HasValue && !string.IsNullOrWhiteSpace(contactInfo.Value.Email);
        if (hasEmail)
        {
            await _emailService.SendOtpEmailAsync(
                contactInfo.Value.Email,
                "رمز التحقق - نظام هويتي الرقمية",
                otpCode,
                "تأكيد الحساب",
                cancellationToken);
        }

        var message = hasEmail
            ? $"OTP verification code sent to your registered email ({contactInfo!.Value.Email}) successfully."
            : "No registered email address was found for this account.";

        // 4. Return success status without exposing OTP code
        return ApiResponse<string>.Success(
            "OTP Sent",
            message: message);
    }
}