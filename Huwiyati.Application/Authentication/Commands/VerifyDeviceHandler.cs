namespace Huwiyati.Application.Authentication.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Authentication.DTOs;

public class VerifyDeviceHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public VerifyDeviceHandler(IApplicationDbContext context, IIdentityService identityService, ITokenService tokenService)
    {
        _context = context;
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<LoginResultDto>> VerifyDeviceAsync(VerifyDeviceCommand command, CancellationToken cancellationToken = default)
    {
        var userId = await _identityService.GetUserIdByNationalNumberAsync(command.NationalNumber, cancellationToken);
        if (!userId.HasValue || userId.Value == Guid.Empty)
        {
            return ApiResponse<LoginResultDto>.Failure("Invalid National Number or code.", statusCode: 400);
        }

        // 1. Verify OTP code
        var verificationCode = await _context.VerificationCodes
            .Where(vc => vc.UserId == userId.Value && !vc.IsUsed)
            .OrderByDescending(vc => vc.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (verificationCode == null || verificationCode.Code != command.Code || verificationCode.ExpirationTime < DateTime.UtcNow)
        {
            return ApiResponse<LoginResultDto>.Failure("Invalid or expired verification code.", statusCode: 400);
        }

        // 2. Mark device as trusted
        var device = await _context.UserDevices
            .FirstOrDefaultAsync(d => d.UserId == userId.Value && d.DeviceIdentifier == command.DeviceIdentifier, cancellationToken);

        if (device == null)
        {
            return ApiResponse<LoginResultDto>.Failure("Device record not found.", statusCode: 404);
        }

        device.IsTrusted = true;
        device.LastLogin = DateTime.UtcNow;
        verificationCode.IsUsed = true;

        await _context.SaveChangesAsync(cancellationToken);

        // 3. Get Citizen Name and Issue JWT Token
        var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == userId.Value, cancellationToken);
        var fullName = person != null ? $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim() : command.NationalNumber;
        var roles = await _identityService.GetUserRolesAsync(userId.Value, cancellationToken);

        var tokenModel = _tokenService.GenerateToken(userId.Value,
                                                     command.NationalNumber,
                                                     fullName,
                                                     "Active",
                                                     roles);

        return ApiResponse<LoginResultDto>.Success(new LoginResultDto
        {
            UserId = userId.Value,
            AccessToken = tokenModel.Token,
            Expiration = tokenModel.Expiration,
            NationalNumber = command.NationalNumber,
            FullName = fullName,
            AccountStatus = "Active",
            RequiresDeviceVerification = false
        }, message: "Device verified successfully. Login completed.");
    }
}