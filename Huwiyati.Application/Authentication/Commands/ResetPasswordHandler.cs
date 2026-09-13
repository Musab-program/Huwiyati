using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Huwiyati.Application.Authentication.Commands;

public class ResetPasswordHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public ResetPasswordHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordCommand command, CancellationToken cancellationToken = default)
    {
        // 1. Get UserId by National Number
        var userId = await _identityService.GetUserIdByNationalNumberAsync(command.NationalNumber, cancellationToken);
        if (!userId.HasValue || userId.Value == Guid.Empty)
        {
            return ApiResponse<bool>.Failure("Invalid National Number or code.", statusCode: 400);
        }

        // 2. Search for valid active verification code
        var verificationCode = await _context.VerificationCodes
            .Where(vc => vc.UserId == userId.Value && !vc.IsUsed)
            .OrderByDescending(vc => vc.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
        if (verificationCode == null || verificationCode.Code != command.Code)
        {
            return ApiResponse<bool>.Failure("Invalid verification code.", statusCode: 400);
        }

        if (verificationCode.ExpirationTime < DateTime.UtcNow)
        {
            return ApiResponse<bool>.Failure("Verification code has expired.", statusCode: 400);
        }

        // 3. Reset Password in Identity
        var resetSucceeded = await _identityService.ResetPasswordAsync(userId.Value, command.NewPassword, cancellationToken);
        if (!resetSucceeded)
        {
            return ApiResponse<bool>.Failure("Failed to reset password. Please ensure password meets security rules.", statusCode: 400);
        }
        // 4. Mark code as used
        verificationCode.IsUsed = true;
        await _context.SaveChangesAsync(cancellationToken);

        // 5. Return success response
        return ApiResponse<bool>.Success(true, message: "Password has been reset successfully.");
    }
}