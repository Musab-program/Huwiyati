using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Authentication;

namespace Huwiyati.Application.Authentication.Commands;

public class ForgotPasswordHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public ForgotPasswordHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<ApiResponse<bool>> RequestPasswordResetAsync(
        ForgotPasswordCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Find UserId by National Number
        var userId = await _identityService.GetUserIdByNationalNumberAsync(command.NationalNumber, cancellationToken);

        // 2. If user exists, generate OTP code valid for 5 minutes and save to Database
            if (userId.HasValue && userId.Value != Guid.Empty)
            {
            var otpCode = new Random().Next(100000, 999999).ToString(); // Generate a 6-digit OTP code
            var verifycationCode = new VerificationCode
            {
                UserId = userId.Value,
                Code = otpCode,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5), // OTP valid for 5 minutes
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _context.VerificationCodes.AddAsync(verifycationCode, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // 3. Return generic security success response
        return ApiResponse<bool>.Success(
            true,
            message: "If an account with this National Number exists, a verification OTP code has been sent.");
    }
}