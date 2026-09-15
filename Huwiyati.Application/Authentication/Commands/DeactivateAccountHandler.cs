namespace Huwiyati.Application.Authentication.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Domain.Enums;

public class DeactivateAccountHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public DeactivateAccountHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<ApiResponse<string>> DeactivateAccountAsync(
        DeactivateAccountCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify credentials
        var userLoginInfo = await _identityService.CheckUserExistAsync(command.NationalNumber, command.Password, cancellationToken);
        if (userLoginInfo == null || !userLoginInfo.Succeeded)
        {
            return ApiResponse<string>.Failure("Invalid National Number or Password.", statusCode: 401);
        }

        // 2. Verify OTP code
        var validCode = await _context.VerificationCodes
            .Where(vc => vc.UserId == userLoginInfo.UserId && !vc.IsUsed)
            .OrderByDescending(vc => vc.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (validCode == null || validCode.Code != command.Code || validCode.ExpirationTime < DateTime.UtcNow)
        {
            return ApiResponse<string>.Failure("Invalid or expired OTP verification code.", statusCode: 400);
        }

        validCode.IsUsed = true;

        // 4. Change account status to Deactivated
        await _identityService.ChangeAccountStatusAsync(userLoginInfo.UserId, AccountStatus.Deactivated, cancellationToken);

        // 5. Untrust all user devices for security
        var devices = await _context.UserDevices
            .Where(d => d.UserId == userLoginInfo.UserId)
            .ToListAsync(cancellationToken);

        foreach (var device in devices)
        {
            device.IsTrusted = false;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ApiResponse<string>.Success("Account deactivated successfully. All devices untrusted.");
    }
}
