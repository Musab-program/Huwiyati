using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Huwiyati.Application.Authentication.Commands;

public class VerifyResetHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    public VerifyResetHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }
    public async Task<ApiResponse<bool>> VerifyResetCodeAsync(VerifyResetCommand command, CancellationToken cancellationToken = default)
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

        // 3. Check if the code is expired
        if (verificationCode.ExpirationTime < DateTime.UtcNow)
        {
            return ApiResponse<bool>.Failure("Verification code has expired. Please request a new one.", statusCode: 400);
        }
        // 4. Return success response
        return ApiResponse<bool>.Success(true, message: "Verification code is valid. You may now reset your password.");
    }
}